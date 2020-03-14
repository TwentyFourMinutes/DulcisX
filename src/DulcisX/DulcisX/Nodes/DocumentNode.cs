using DulcisX.Core.Enums;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StringyEnums;

namespace DulcisX.Nodes
{
    public class DocumentNode : ProjectItemNode, IPhysicalNode
    {
        public override NodeTypes NodeType => NodeTypes.Document;

        public DocumentNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }

        public DocumentNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project, itemId)
        {
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentProject.UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }

        public string GetBuildAction()
            => HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID4.VSHPROPID_BuildAction);

        public void SetBuildAction(string buildAction)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingHierarchy.SetProperty(ItemId, (int)__VSHPROPID4.VSHPROPID_BuildAction, buildAction);

            ErrorHandler.ThrowOnFailure(result);
        }

        public CopyToOutputDirectory GetCopyToOutputDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var val = ParentProject.GetItemProperty(ItemId, DocumentProperty.CopyToOutputDirectory);

            return val.GetEnumFromRepresentation<CopyToOutputDirectory>();
        }
    }
}
