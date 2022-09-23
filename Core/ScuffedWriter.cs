using System.Numerics;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ForgeTools.Core;

public class ScuffedWriter
{
    public static void WritePosToItem(XElement xElement, Vector3 pos)
    {
        xElement.XPathSelectElement("/struct[@id='3']/float[@id='0']").Value = pos.X.ToString();
        xElement.XPathSelectElement("/struct[@id='3']/float[@id='1']").Value = pos.Y.ToString();
        xElement.XPathSelectElement("/struct[@id='3']/float[@id='2']").Value = pos.Z.ToString(); 
        
    }
}