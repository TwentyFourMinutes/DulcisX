namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a Node which exists physically on disk.
    /// </summary>
    public interface IPhysicalNode : INamedNode
    {
        /// <summary>
        /// Returns the name of the file, including the extension.
        /// </summary>
        /// <returns>A string containg the name of the file.</returns>
        string GetFileName();
        /// <summary>
        /// Returns the full name of the file.
        /// </summary>
        /// <returns>A string containg the full name of the file.</returns>
        string GetFullName();

        string GetDirectoryName();
    }
}
