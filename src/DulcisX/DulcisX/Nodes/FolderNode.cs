using DulcisX.Core.Models.Enums;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Nodes
{
    public class FolderNode : ProjectItemNode, IPhysicalNode
    {
        public override NodeTypes NodeType => NodeTypes.Folder;

        public FolderNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }

        public FolderNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project, itemId)
        {
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentProject.UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }
    }
}
