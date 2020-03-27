namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Passed to <see cref="Nodes.ProjectNode.GetItemProperty(uint, DocumentProperty)"/> or <see cref="Nodes.ProjectNode.SetItemProperty(uint, DocumentProperty, string)"/>, to specify the property of which the value should be set or retrieved.
    /// </summary>
    public enum DocumentProperty
    {
        /// <summary>
        /// Specifies the Copy to Output Directory property.
        /// </summary>
        CopyToOutputDirectory
    }
}
