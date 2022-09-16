using System.Numerics;

namespace ForgeTools.Core;

public class Transform
{
    public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    /// <summary>
    /// Items <b>X/Y/Z</b> position in the world  
    /// 
    /// <remarks><i>This get <b>divided by 10</b> when saved to the mvar file. This represents <b>list id 23</b> inside the object element
    /// <a href="https://gist.github.com/joshf67/7cc15f31e54db7466671d84e4f9b1630"> reference</a> </i> </remarks>
    /// </summary>
    /// 
    public Vector3 Position { get; set; }

    /// <summary>
    /// Items <b>X/Y/Z</b> Scale only works when item is static 
    /// 
    /// <remarks><i>this gets <b>divided by 4</b> when saved to the mvar file.
    /// This represents <b>list id 23</b> inside the object element
    /// <a href="https://gist.github.com/joshf67/7cc15f31e54db7466671d84e4f9b1630"> reference</a> </i> </remarks>
    /// </summary>
    /// 
    public Vector3 Scale { get; set; }
    /// <summary>
    /// Items <b>X/Y/Z</b> Rotation
    /// 
    /// <remarks><i>this gets converted to a forward and upward vector when saved to the mvar file.
    /// This represents <b>list id 4 and 5</b> inside the object element
    /// <a href="https://gist.github.com/joshf67/7cc15f31e54db7466671d84e4f9b1630"> reference</a> </i> </remarks>
    /// </summary>
    /// 
    public Vector3 Rotation { get; set; }


    public Transform()
    {
        Vector3 v = new Vector3(90, 0, 0);
        Vector3.Transform(v,)
    }
}