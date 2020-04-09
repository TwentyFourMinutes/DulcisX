
namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents a Node which has a name.
    /// </summary>
    public interface INamedNode : IBaseNode
    {
        /// <summary>
        /// Returns the name displayed in the Visual Studio Solution Explorer.
        /// </summary>
        /// <returns>A string containg the display name of the Node.</returns>
        string GetDisplayName();
    }
}
