using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using InfiniteForgeConstants;
using InfiniteForgePacker.XML.Misc;
using Serilog;

namespace ForgeTools;

public class TransformTemp : Transform //this will get replaced once base class is finished
{
    public Vector3 Forward { get; set; }
    public Vector3 Up { get; set; }
}

public class Util
{
    public static Vector3 GetForwardVectorFromRotation(Vector3 rotation)
    {
        Quaternion q = Quaternion.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
        Vector3 v = new Vector3(0, 1, 0);


        return Vector3.Transform(v, q);
    }

    public static Vector3 GetUpVectorFromRotation(Vector3 rotation)
    {
        Quaternion q = Quaternion.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
        Vector3 v = new Vector3(0, 0, 1);

        return Vector3.Transform(v, q);
    }

    public static XDocument LoadXml(string path)
    {
        Log.Information("Loading XML from {Path}", path);
        return XDocument.Load(path);
    }

    public static DMap BuildMapFromXML(string path)
    {
        DMap map = new DMap();
        XDocument rawXml = LoadXml(path);

        var objectList = GetObjectPropertiesList(rawXml);
        foreach (var item in objectList.Elements())
        {
            DGameObject gameObject = new DGameObject();
            var transform = GetTransform(item);
            gameObject.Forward = transform.forward;
            gameObject.Scale = transform.scale;
            gameObject.IsStatic = transform.isStatic;
            gameObject.Up = transform.up;
            gameObject.Position = transform.positon;
            map.GameObjects.Add(gameObject);

            Log.Information("{Element}", item);
        }

        return map;
    }


    public static (Vector3 positon, Vector3 forward, Vector3 up, Vector3 scale, bool isStatic) GetTransform(
        XElement item)
    {
        Vector3 forward = Vector3.Zero;
        Vector3 up = Vector3.Zero;
        Vector3 position = Vector3.Zero;
        Vector3 scale = Vector3.Zero;
        bool isStatic = false;
        var elements = item.Elements();

        foreach (var e in elements)
        {
            var att = e.Attribute("id");
            if (att.Value == "3") //Get the position struct and fill in transform 
            {
                position = ParseXmlVector3(e);
            }

            if (att.Value == "4") // up
            {
                up = ParseXmlVector3(e);
            }

            if (att.Value == "5") // forward
            {
                forward = ParseXmlVector3(e);
            }

            if (att.Value == "8") // scale / static
            {
                

                foreach (var s8Elements in e.Elements())
                {
                    
                    if (s8Elements.Attribute("id").Value == "24")
                    {
                        
                        if (s8Elements.Element("struct").Element("int32").Value == "2")
                        {
                            isStatic = true;
                        }

                        if (s8Elements.Element("struct").Element("int32").Value == "1")
                        {
                            isStatic = false;
                        }
                    }
                }
                
                
            }

            
        }


        return (positon: position, forward: forward, up: up, scale: scale, isStatic: isStatic);
    }

    public static Vector3 ParseXmlVector3(XElement parentElement)
    {
        Vector3 v = Vector3.Zero;

        foreach (var axisElement in parentElement.Elements())
        {
            var axisId = axisElement.Attribute("id").Value;

            switch (axisId)
            {
                case "0": // X 
                    v.X = float.Parse(axisElement.Value);
                    break;
                case "1": // Y
                    v.Y = float.Parse(axisElement.Value);
                    break;
                case "2": // Z
                    v.Z = float.Parse(axisElement.Value);
                    break;
            }
        }

        return v;
    }

    public static XElement GetObjectPropertiesList(XDocument xml)
    {
        return xml.Root.Element("list");
    }
}