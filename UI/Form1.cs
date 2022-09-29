using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;
using Bond;
using BondReader;
using BondReader.Schemas;
using BondReader.Schemas.Generic;
using BondReader.Schemas.Items;
using ForgeTools;
using ForgeTools.Core;
using InfiniteForgeConstants;
using InfiniteForgeConstants.MapSettings;
using InfiniteForgeConstants.ObjectSettings;
using InfiniteForgePacker.XML;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
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
        // DCMap.LoadMap(XDocument.Load(Program.EXEPath + "/DefaultStaticPrimitiveCube Variant 16x16x16.mvar.xml"));


        // var bonded = InfiniteForgePacker.XML.  Bonded<BondSchema>

        // var Bond = BondHelper.ProcessFile<IBonded<BondSchema>>(Program.EXEPath +
        //                                                          "/testmap.mvar"); // new BondSchema(MapId.BEHEMOTH);
        var map = new BondSchema();
        // BondSchema m = new BondSchema();


        var random = new Random();
        for (int i = 0; i < 1200; i++)
        {
            var randonPos = new Vector3(
                (-1389 + (random.Next(-25, 25))) / 10f,
                (709 + (random.Next(-25, 25))) / 10f,
                (100 + 250 + (random.Next(0, 25))) / 10f
            );

            var randomRot = new Vector3(random.Next(-180, 180), random.Next(-180, 180), random.Next(-180, 180));

            Transform transform = new Transform(randonPos, randomRot);

            transform.PhysicsType = PhysicsType.NORMAL;
            var go = new GameObject(transform: transform);

            go.ObjectSettings ??= new AdditionalObjectSettings(-1847613636);


            ObjectId[] sphereVariants = new[]
            {
                ObjectId.FUSION_COIL_PLASMA, ObjectId.FUSION_COIL_SHOCK, ObjectId.FUSION_COIL_KINETIC
            }; //{ 329774963, -1868408199, 11784207, -192867617 };
            var randonVariant = random.Next(0, 3);


            go.Transform.IsStatic = false;
            go.ObjectId = ObjectId.FUSION_COIL_PLASMA;
            go.ObjectId = sphereVariants[randonVariant];


            ItemSchema itemSchema = new ItemSchema(go);

            itemSchema.SettingsContainer.Scale.RemoveFirst();
            map.Items.AddLast(itemSchema);
        }

        map.MapIdContainer.MapId.Int = (int)InfiniteForgeConstants.MapSettings.MapId.BEHEMOTH;
        // BondHelper.WriteBond(map, Program.EXEPath + "/testmapSaved22222222.mvar");
        BondHelper.WriteBond(map, "C:/Halo Infinite Insider/__cms__/rtx-new/variants/Test.mvar");
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
            var mapObject = new Map(MapId.BEHEMOTH);
            var fileInfo = new FileInfo(fileDialog.FileName);
            Program.Settings.LastUsedPath = fileDialog.FileName;
            Log.Information(fileDialog.FileName);
            AddFileToButtonLayout(fileInfo);

            // var map = new Map();

            //var mapShell = XDocument.Load(Program.EXEPath + "/ExampleMap.xml");
            string[] items = File.ReadAllText(fileInfo.FullName).Split("}", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var i = item + "}";


                var bItem = JsonConvert.DeserializeObject<BlenderItem>(i);
                if (bItem == null)
                    continue;
                GameObject go = new GameObject();
                go.Transform.Position.X = bItem.PositionX + 1389;
                go.Transform.Position.Y = bItem.PositionY + 709;
                go.Transform.Position.Z = bItem.PositionZ + 100;

                go.Transform.Scale.X = bItem.ScaleX / 4;
                go.Transform.Scale.Y = bItem.ScaleY / 4;
                go.Transform.Scale.Z = bItem.ScaleZ / 4;
                go.Transform.IsStatic = true;

                var rot = Vector3.Zero;
                rot.X = bItem.RotationX;
                rot.Y = bItem.RotationY;
                rot.Z = bItem.RotationZ;

                go.Transform.EulerRotation = rot;
                //mapItem.Transform.DirectionVectors

                go.ObjectId = ObjectId.PRIMITIVE_BLOCK;

                mapObject.GameObjects.Add(go);
                //  XMLHelper.AddObject(mapShell, mapItem.GameObject);
            }

            BondSchema map = new BondSchema(mapObject);

            BondHelper.WriteBond<BondSchema>(map, Program.EXEPath + "/BlenderMap.mvar");
            //mapShell.WriteTo(XmlWriter.Create(Program.EXEPath + "/Test.xml"));
        }
    }
}