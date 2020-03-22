using DulcisX.Core.Enums;
using DulcisX.Exceptions;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StringyEnums;
using System;
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

        public string GetFileName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID.VSHPROPID_Name);
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

        public DocumentNode Rename(string newName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var fullName = GetFullName();

            if (!Path.HasExtension(newName))
            {
                newName += Path.GetExtension(fullName);
            }

            var newFullName = Path.Combine(Path.GetDirectoryName(fullName), newName);

            File.Move(fullName, newFullName);

            var project = GetParentProject();

            if (!project.TryRemoveChildren(this, out var code))
            {
                ErrorHandler.ThrowOnFailure(code);
            }

            var parentId = this.GetParentNodeId();

            var success = project.AddExistingDocument(parentId, newFullName);

            if (success != VSADDRESULT.ADDRESULT_Success)
            {
                throw new OperationNotSuccessfulException($"Couldn't re-add the file to the project. AddResult: '{success}'.");
            }

            GetParentProject().TryGetDocumentNode(newFullName, out var document);

            return document;
        }
    }
}
