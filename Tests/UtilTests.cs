using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;

namespace ForgeTools.Tests;

public class UtilTests
{
    private static string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    private static XDocument LoadXml(string relativeFilePath)
    {
        var file = path + relativeFilePath;
        return XDocument.Load(file);
    }

    [Theory]
    [InlineData("/Resources/TestMaps/xml/ScaledCubeMapTest.mvar.xml","")]
    public void GetObjectPropertyList_ShouldReturn_ObjectPropertyListXElement(string relativeFilePath,string expected)
    {
        var xml = LoadXml(relativeFilePath);
        var result = MapVariantHelper.GetObjectPropertyList(xml);
        result.Name.ToString().Should().Be("list");
        result.FirstAttribute.Value.Should().Be("3");
    }

    [Theory]
    [InlineData("/Resources/TestMaps/xml/EmptyMapTest.mvar.xml")]
    public void GetObjectPropertyList_ShouldThrowException_BecauseObjectPropertyListXElementNotPresent
        (string relativeFilePath)
    {
        XDocument xml = LoadXml(relativeFilePath);


        var resultFunc = () => MapVariantHelper.GetObjectPropertyList(xml);

        resultFunc.Should().Throw<Exception>();
    }
}