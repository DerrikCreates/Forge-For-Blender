using System.Numerics;

namespace ForgeTools;

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
}

