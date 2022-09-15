using System.Xml.Linq;
using Serilog;


namespace ForgeTools;

public static class MapVariantHelper
{
    public static XElement CreateObject(float xPos, float yPos, float zPos)
    {
        XElement NewObject = new XElement("struct");

        var position = new XElement("struct");
        position.SetAttributeValue("id", 3);

        var x = new XElement("float");
        x.SetAttributeValue("id", 0);
        x.Value = (-137.8769 + xPos).ToString();

        var y = new XElement("float");
        y.SetAttributeValue("id", 1);
        y.Value = (42.940845 + yPos).ToString();

        var z = new XElement("float");
        z.SetAttributeValue("id", 2);
        z.Value = (14.945889 + zPos).ToString();

        position.Add(x);
        position.Add(y);
        position.Add(z);

        var modelContainer = new XElement("struct");
        modelContainer.SetAttributeValue("id", 2);

        var modelID = new XElement("int32");
        modelID.Value = "1759788903";

        modelContainer.Add();
        modelContainer.Add(modelID);
        NewObject.Add(modelContainer);
        NewObject.Add(position);


        return NewObject;
    }


    public static XDocument ReadXmlFile(string pathToFile)
    {
        var xml = File.ReadAllText(pathToFile);
        return XDocument.Load(xml);
    }

    public static XElement GetObjectPropertyList(XDocument xml)
    {
        if (xml.Root == null)
        {
            Log.Error("xml file's root node is null");
            throw new NullReferenceException("XML File's root element is null");
        }

        var element = xml.Root.Element("list");
        if (element == null)
        {
            Log.Error("Could not find Object Property XElement");
            throw new Exception("Could not find Object Property XElement");
        }

        return element;
    }
}