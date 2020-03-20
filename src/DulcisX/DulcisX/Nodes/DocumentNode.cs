using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using DulcisX.Core.Extensions;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StringyEnums;
using System.IO;

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

            var result = GetParentProject().UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }

        public string GetBuildAction()
            => HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID4.VSHPROPID_BuildAction);

        public CopyToOutputDirectory GetCopyToOutputDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var val = GetParentProject().GetItemProperty(ItemId, DocumentProperty.CopyToOutputDirectory);

            return val.GetEnumFromRepresentation<CopyToOutputDirectory>();
        }

        public void SetCopyToOutputDirectory(CopyToOutputDirectory copyToOutputDirectory)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            GetParentProject().SetItemProperty(ItemId, DocumentProperty.CopyToOutputDirectory, copyToOutputDirectory.GetRepresentation());
        }

        public void Rename(string newName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = ParentSolution.ServiceContainer.GetCOMInstance<IVsRunningDocumentTable>();

            var fullName = GetFullName();

            var name = Path.GetFileName(fullName);

            if (!Path.HasExtension(newName))
            {
                newName += Path.GetExtension(fullName);
            }

            var result = rdt.RenameDocument(fullName, fullName.Replace(name, newName), VSConstants.HIERARCHY_DONTCHANGE, CommonNodeIds.Nil);

            ErrorHandler.ThrowOnFailure(result);
        }

        public void ChangeExtension(string extension)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = ParentSolution.ServiceContainer.GetCOMInstance<IVsRunningDocumentTable>();

            var fullName = GetFullName();

            var result = rdt.RenameDocument(fullName, Path.ChangeExtension(fullName, extension), VSConstants.HIERARCHY_DONTCHANGE, CommonNodeIds.Nil);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}
