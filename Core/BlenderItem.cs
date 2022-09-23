using Newtonsoft.Json;

namespace ForgeTools.Core;

public class BlenderItem
{
    [JsonProperty("positionX")] public float  PositionX;
    [JsonProperty("positionY")] public float PositionY;
    [JsonProperty("positionZ")] public float PositionZ;

    [JsonProperty("scaleX")] public float ScaleX;
    [JsonProperty("scaleY")] public float ScaleY;
    [JsonProperty("scaleZ")] public float ScaleZ;

    [JsonProperty("rotationX")] public float RotationX;
    [JsonProperty("rotationY")] public float RotationY;
    [JsonProperty("rotationZ")] public float RotationZ;

    /*
    positionX = None
    positionY = None
    positionZ = None

    scaleX = None
    scaleY = None
    scaleZ = None

    rotationX = None
    rotationY = None
    rotationZ = None
    */
}