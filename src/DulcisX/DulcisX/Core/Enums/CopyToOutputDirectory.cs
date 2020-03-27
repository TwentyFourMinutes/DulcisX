using StringyEnums;

namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Specifies the values of the <see cref="DocumentProperty.CopyToOutputDirectory"/> passed to the <see cref="Nodes.DocumentNode.GetCopyToOutputDirectory"/> or  <see cref="Nodes.DocumentNode.SetCopyToOutputDirectory(CopyToOutputDirectory)"/>.
    /// </summary>
    public enum CopyToOutputDirectory
    {
        /// <summary>
        /// Specifies that the document will be copied to the output directory.
        /// </summary>
        [StringRepresentation("Always")]
        Always,
        /// <summary>
        /// Specifies that the document will be copied to the output directory, if the source file is newer.
        /// </summary>
        [StringRepresentation("PreserveNewest")]
        IfNewer,
        /// <summary>
        /// Specifies that the document will not be copied to the output directory.
        /// </summary>
        [StringRepresentation("Never")]
        Never
    }
}
