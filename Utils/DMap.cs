using InfiniteForgeConstants;

namespace ForgeTools;

public class DMap // most of what i do here should get merged into infinite forge constants
{
    public MapId Type { get; set; }
    public List<DGameObject> GameObjects { get; set; } = new List<DGameObject>();
}