using StringyEnums;

namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Common stream keys in the .suo file. Passed to <see cref="Nodes.SolutionNode.LoadUserConfiguration(Microsoft.VisualStudio.Shell.Interop.IVsPersistSolutionOpts, string)"/>.
    /// </summary>
    public enum CommonStreamKey
    {
        /// <summary>
        /// Specifies the solution configuration strean inside the .suo file.
        /// </summary>
        [StringRepresentation("SolutionConfiguration")]
        SolutionConfiguration
    }
}
