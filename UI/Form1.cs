using System.Numerics;
using System.Xml;
using System.Xml.Linq;
using BondReader;
using BondReader.Schemas;
using ForgeTools;
using ForgeTools.Core;
using InfiniteForgeConstants;
using InfiniteForgeConstants.ObjectSettings;
using InfiniteForgePacker.XML;
using Newtonsoft.Json;
using Schemas;
using Serilog;
using Vector3 = System.Numerics.Vector3;

namespace DerriksForgeTools;

public partial class Form1 : Form
{
    private Dictionary<Button, string> fileButtons = new Dictionary<Button, string>();

    public Form1()
    {
        InitializeComponent();
    }

    private string _activeFile;

    private void OnFileButtonClick(object? sender, EventArgs e)
    {
        var path = fileButtons[(Button)sender];
    }


    private void DisplayMapData(string xmlPath)
    {
        // ADD Displaying of map data here. mostly for testing
        Log.Debug("display map data method {XMLPath}", xmlPath);
    }

    private void AddFileToButtonLayout(FileInfo path)
    {
        if (fileButtons.ContainsValue(path.FullName) == false)
        {
            var b = new Button();
            b.Text = path.Name;
            FileListLayoutPanel.Controls.Add(b);
            fileButtons.Add(b, path.FullName);
            b.Click += OnFileButtonClick;
        }
    }


    private void button1_Click(object sender, EventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.InitialDirectory = Program.Settings.LastUsedPath;
        fileDialog.Filter = "xml files (*.xml)|*.xml";

        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            var fileInfo = new FileInfo(fileDialog.FileName);
            Program.Settings.LastUsedPath = fileDialog.FileName;
            Log.Information(fileDialog.FileName);
            AddFileToButtonLayout(fileInfo);

            var obj = XMLObject.GenerateObjectFromXML
                (XElement.Load(Program.EXEPath + "/ExampleStaticItem.xml"));

            //XMLMap.WriteMap();

            var userMap = XMLMap.GenerateMapFromXML(XDocument.Load(fileInfo.FullName));


            label_MapName.Text = userMap.Map.MapId.ToString();
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        DCMap.LoadMap(XDocument.Load(Program.EXEPath + "/DefaultStaticPrimitiveCube Variant 16x16x16.mvar.xml"));


        var map = BondHelper.ProcessFile<MapSchema>(Program.EXEPath + "/testmap.mvar");
        var random = new Random();
        for (int i = 0; i < 6500; i++)
        {
            var item = new Item();

            item.Position.X = (-1238 + random.Next(-350, 350)) / 10f;
            item.Position.Y = (537 + random.Next(-350, 350)) / 10f;
            item.Position.Z = (100 + random.Next(-350, 350)) / 10f;
            item.ItemId = new GenericIntStruct() { Int = (int)ObjectId.PRIMITIVE_BLOCK };
            map.Items.AddFirst(item);

            var trans = new Transform(Vector3.Zero,
                new Vector3(random.Next(-180, 180), random.Next(-180, 180), random.Next(-180, 180)));


            item.Forward.X = trans.DirectionVectors.Forward.X;
            item.Forward.Y = trans.DirectionVectors.Forward.Y;
            item.Forward.Z = trans.DirectionVectors.Forward.Z;

            item.Up.X = trans.DirectionVectors.Up.X;
            item.Up.Y = trans.DirectionVectors.Up.Y;
            item.Up.Z = trans.DirectionVectors.Up.Z;

            item.StaticDynamicFlagUnknown = (byte)21;
            var variantSettings = new Item.UnknownVariantSettings
            {
                StaticDynamicFlag = 2,
                ScriptBrainFlag = 1
            };
            ItemSettingsContainer.ScaleList scale = new ItemSettingsContainer.ScaleList();
            scale.ScaleContainer = new BondReader.Schemas.Vector3();
            scale.ScaleContainer.X = (random.Next(2, 15) / 4f);
            scale.ScaleContainer.Y = (random.Next(2, 15) / 4f);
            scale.ScaleContainer.Z = (random.Next(2, 15) / 4f); 
            item.SettingsContainer.Scalelist.AddFirst(scale);
 
            item.VariantSettingsList.AddFirst(variantSettings);
        }

        map.MapIdContainer.MapId.Id = (int)InfiniteForgeConstants.MapSettings.MapId.BEHEMOTH;
        BondHelper.WriteBond(map, Program.EXEPath + "/testmapSaved22222222.mvar");
    }

    private void button2_Click(object sender, EventArgs e)
    {
    }

    private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void button_LoadBlenderJson_Click(object sender, EventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.InitialDirectory = Program.Settings.LastUsedPath;
        fileDialog.Filter = "Blender Export files (*.DCjson)|*.DCjson";

        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            var fileInfo = new FileInfo(fileDialog.FileName);
            Program.Settings.LastUsedPath = fileDialog.FileName;
            Log.Information(fileDialog.FileName);
            AddFileToButtonLayout(fileInfo);

            // var map = new Map();

            var mapShell = XDocument.Load(Program.EXEPath + "/ExampleMap.xml");
            var itemsContainer = XMLHelper.GetObjectList(mapShell);
            string[] items = File.ReadAllText(fileInfo.FullName).Split("}", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var itemShell = XDocument.Load(Program.EXEPath + "/ExampleStaticItem.xml").Root;

                var i = item + "}";
                var mapItem =
                    XMLObject.GenerateObjectFromXML(XDocument.Load(Program.EXEPath + "/ExampleStaticItem.xml"));

                var bItem = JsonConvert.DeserializeObject<BlenderItem>(i);
                if (bItem == null)
                    continue;

                mapItem.GameObject.Transform.Position.X = bItem.PositionX;
                mapItem.GameObject.Transform.Position.Y = bItem.PositionY;
                mapItem.GameObject.Transform.Position.Z = bItem.PositionZ;

                mapItem.GameObject.Transform.Scale.X = bItem.ScaleX / 4;
                mapItem.GameObject.Transform.Scale.Y = bItem.ScaleY / 4;
                mapItem.GameObject.Transform.Scale.Z = bItem.ScaleZ / 4;
                mapItem.GameObject.Transform.IsStatic = true;

                var rot = Vector3.Zero;
                rot.X = bItem.RotationX;
                rot.Y = bItem.RotationY;
                rot.Z = bItem.RotationZ;

                mapItem.GameObject.Transform.EulerRotation = rot;
                //mapItem.Transform.DirectionVectors

                mapItem.GameObject.ObjectId = ObjectId.PRIMITIVE_BLOCK;


                //  XMLHelper.AddObject(mapShell, mapItem.GameObject);


                mapItem.WriteObject(itemShell);

                itemsContainer.Add(itemShell);
            }

            mapShell.Save(Program.EXEPath + "/Test.xml");
            //mapShell.WriteTo(XmlWriter.Create(Program.EXEPath + "/Test.xml"));
        }
    }
}