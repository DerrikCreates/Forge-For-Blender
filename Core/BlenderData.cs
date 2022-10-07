using System.Numerics;
using BondReader;
using BondReader.Schemas;
using InfiniteForgeConstants;
using InfiniteForgeConstants.MapSettings;
using InfiniteForgeConstants.ObjectSettings;
using Newtonsoft.Json;
using Serilog;

namespace ForgeTools.Core;

public static class BlenderData
{
    public static void ProcessAndSave(string dataPath, string outputPath)
    {
        var mapObject = new Map(MapId.CATALYST); //todo read map id from blender
        var fileInfo = new FileInfo(dataPath);

        Log.Information("Saving Mvar to {OutputPath}", outputPath);


        // var map = new Map();

        //var mapShell = XDocument.Load(Program.EXEPath + "/ExampleMap.xml");
        string[] items = File.ReadAllText(fileInfo.FullName).Split("}", StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in items)
        {
            var i = item + "}";


            var bItem = JsonConvert.DeserializeObject<BlenderItem>(i);
            if (bItem == null)
                continue;


            var forward = new Vector3(bItem.ForwardX, bItem.ForwardY, bItem.ForwardZ);
            var up = new Vector3(bItem.UpZ, bItem.UpY, bItem.UpX);


            Transform t = new Transform(Vector3.Zero, (up, forward));
            GameObject go = new GameObject(transform: t);


            go.Transform.Position.X = (bItem.PositionX); // / 10f;
            go.Transform.Position.Y = (bItem.PositionY); // / 10f;
            go.Transform.Position.Z = (bItem.PositionZ); /// 10f;

            go.Transform.Scale.X = bItem.ScaleX; /// 4f;
            go.Transform.Scale.Y = bItem.ScaleY; /// 4f;
            go.Transform.Scale.Z = bItem.ScaleZ; // / 4f;
            go.Transform.IsStatic = true;


            go.ObjectId = ObjectId.PRIMITIVE_CONE;

            mapObject.GameObjects.Add(go);
            //  XMLHelper.AddObject(mapShell, mapItem.GameObject);
        }


        BondSchema map = new BondSchema(mapObject);

        BondHelper.WriteBond(map, outputPath + "/BlenderMap.mvar");
        //mapShell.WriteTo(XmlWriter.Create(Program.EXEPath + "/Test.xml"));
    }
}