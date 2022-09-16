// ReSharper disable All

using DataTypes.Forge;
using InfiniteForgePacker.Game;

namespace ForgeTools.Core;

public class ForgeItem
{
    /// <summary>
    ///  This determins what item this actaully is.
    /// </summary>
    public ObjectId ItemId { get; set; }
    /// <summary>
    /// The rotation position and scale of the object
    /// </summary>
    public Transform Transform { get; set; }

    public AdditionalItemSettings AdditionalSettings { get; set; }      
   

    //TODO add other item properties like material ids and color
}