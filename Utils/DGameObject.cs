using System.Numerics;
using InfiniteForgeConstants;

namespace ForgeTools;

public class DGameObject
{
    public ObjectId ObjectId { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Forward { get; set; }
    public Vector3 Up { get; set; }
    public bool IsStatic { get; set; }
}