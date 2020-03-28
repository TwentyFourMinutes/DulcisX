using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a Solution Folder within a <see cref="SolutionNode"/>.
    /// </summary>
    public class SolutionFolderNode : SolutionItemNode
    {
        /// <inheritdoc/>
        public override NodeTypes NodeType => NodeTypes.SolutionFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionFolderNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="SolutionFolderNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the <see cref="SolutionFolderNode"/> itself.</param>
        public SolutionFolderNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy, CommonNodeIds.SolutionFolder)
        {
        }
    }
}
