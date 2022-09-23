using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Xml.XPath;
using InfiniteForgeConstants;
using InfiniteForgeConstants.MapSettings;
using InfiniteForgeConstants.ObjectSettings;
using InfiniteForgePacker.XML;
using Newtonsoft.Json;
using Serilog;

namespace ForgeTools.Core;

public class DCMap
{
    [DCPath("/struct/list/struct/struct[@id='2']/int32")]
    public MapId MapId { get; set; }

    [DCParse] public DCTransform Transform { get; set; } = new DCTransform();
    
   
    
    public List<GameObject> GameObjects { get; set; }

    public static DCMap LoadMap(XDocument xml)
    {
        DCMap map = new DCMap();

        PropertyInfo[] properties = map.GetType().GetProperties();


        foreach (var prop in properties)
        {
            ProcessProp(map, prop, xml);
            /*
            var subProps = prop.PropertyType.GetCustomAttributes();
            foreach (var subProp in subProps)
            {
                if (subProp.GetType() == typeof(DCPathAttribute)) // process the data
                {
                    ProcessProp(map);
                }

                if (subProp.GetType() == typeof(DCParseAttribute)) // go deeper into the object and parse its children
                {
                }
                */
        }


        return map;

        /*private static object GetNextProp(object instance)
        {
            var t = instance.GetType().GetProperties();
            var a = instance.GetType().GetCustomAttributes(typeof(DCBaseAttribute));
    
            t.SetValue(instance, 1);
    
    
            return null;
        }
        
    
        private static void LoadData()
        {
        }
    
        public static XDocument WriteMap(DCMap map)
        {
            XDocument xDocument = new XDocument();
            PropertyInfo[] properties = Type.GetType("ForgeTools.Core.DCMap").GetProperties();
    
            foreach (var prop in properties)
            {
                var attrs = prop.GetCustomAttributes(true);
    
                foreach (var attr in attrs)
                {
                    DCPathAttribute dcPathAttribute = attr as DCPathAttribute;
                    if (dcPathAttribute != null)
                    {
                        map.GetType().GetProperty(prop.Name).SetValue(map, -1449092339);
                    }
                }
            }
    
            return xDocument;
        }
        */
    }

    private static void ProcessProp(object instance, PropertyInfo currentProp, XDocument xml)
    {
        var currentAttrs = currentProp.GetCustomAttributes(typeof(DCBaseAttribute));
        if (currentAttrs.Count() < 1)
            return;
        var attr = currentAttrs.First();
        if (attr is DCParseAttribute)
        {
            var currentPropInstance = currentProp.GetValue(instance);
            // GO DEEPER TO FIND DATA
            var props = currentPropInstance.GetType().GetProperties(); // grabs the properties properties

            foreach (var prop in props)
            {
                ProcessProp(currentPropInstance, prop, xml);
            }
        }

        if (attr is DCPathAttribute)
        {
            // COLLECT DATA


            foreach (var att in currentAttrs)
            {
                if (attr is DCPathAttribute attributeWithData)
                {
                    Type t = currentProp.PropertyType;
                    var converter = TypeDescriptor.GetConverter(t);


                    var xmlDataToParse = xml.XPathSelectElement(attributeWithData.XPath).Value;

                    object final =
                        converter.ConvertFromString(xmlDataToParse); //converter.ConvertFromString(xmlDataToParse);

                    currentProp.SetValue(instance, final);
                }
            }
        }

        static void LoadPropFromFile(PropertyInfo prop, object instance, XDocument xml)
        {
            var attrs = prop.GetCustomAttributes(true);

            foreach (var attr in attrs)
            {
                DCPathAttribute dcPathAttribute = attr as DCPathAttribute;
                if (dcPathAttribute != null)
                {
                    /*
                    string s = xml.XPathSelectElement(dcPathAttribute.XPath).Value;
                    var pType = prop.PropertyType;
                    // var o = Convert.ChangeType(s, prop.PropertyType);

                    var converter = TypeDescriptor.GetConverter(pType);
                    object final = converter.ConvertFromString(s);
                    */

                    //  map.GetType().GetProperty(prop.Name)
                    //      .SetValue(map, MapId.BEHEMOTH); // prop.SetValue(prop, MapId.BEHEMOTH );
                }
            }
        }
    }
}