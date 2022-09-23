using System.Numerics;

namespace ForgeTools.Core;

public class DCTransform
{
    [DCPath("/struct/list/struct/struct[@id='3']/float[@id='0']")]
    public float xPos { get; set; }

    [DCPath("/struct/list/struct/struct[@id='3']/float[@id='1']")]
    public float yPos { get; set; }

    [DCPath("/struct/list/struct/struct[@id='3']/float[@id='2']")]
    public float zPos { get; set; }


    public Vector3 Position => new Vector3(xPos, yPos, zPos);
}