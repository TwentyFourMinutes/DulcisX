using Microsoft.VisualStudio;

namespace DulcisX.Core.Enums.VisualStudio
{
    /// <summary>
    /// Common nodes inside an <see cref="Microsoft.VisualStudio.Shell.Interop.IVsHierarchy"/>.
    /// </summary>
    public static class CommonNodeIds
    {
        /// <summary>
        /// Represents no node.
        /// </summary>
        public const uint Nil = VSConstants.VSITEMID_NIL;
        /// <summary>
        /// Represents a node selection of a collection whic is larger or equal to two.
        /// </summary>
        public const uint MutlipleSelectedNodes = VSConstants.VSITEMID_SELECTION;
        /// <summary>
        /// Represents the root node of the hierarchy.
        /// </summary>
        public const uint Root = VSConstants.VSITEMID_ROOT;
        /// <summary>
        /// Represents a <see cref="Nodes.ProjectNode"/>.
        /// </summary>
        public const uint Project = Root;
        /// <summary>
        /// Represents a <see cref="Nodes.SolutionFolderNode"/>.
        /// </summary>
        public const uint SolutionFolder = Root;
        /// <summary>
        /// Represents a <see cref="Nodes.SolutionNode"/>.
        /// </summary>
        public const uint Solution = Root;
    }
}
