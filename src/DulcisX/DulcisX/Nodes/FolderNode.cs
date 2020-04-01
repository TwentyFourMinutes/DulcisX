using DulcisX.Core.Enums;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a folder within a <see cref="ProjectNode"/>.
    /// </summary>
    public class FolderNode : ProjectItemNode, IPhysicalNode
    {
        /// <inheritdoc/>
        public override NodeTypes NodeType => NodeTypes.Folder;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="FolderNode"/> sits in.</param>
        /// <param name="project">The Project in which the <see cref="FolderNode"/> sits in.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="FolderNode"/> in the <paramref name="project"/>.</param>
        public FolderNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project, itemId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="FolderNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the Project in which the <see cref="FolderNode"/> sits in.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="FolderNode"/> in the <paramref name="hierarchy"/>.</param>

        public FolderNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }

        /// <inheritdoc/>
        public string GetFileName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID.VSHPROPID_Name);
        }

        /// <inheritdoc/>
        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = GetParentProject().UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }

        public string GetDirectoryName()
            => GetFullName();
    }
}
