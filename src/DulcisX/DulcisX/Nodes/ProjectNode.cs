using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using DulcisX.Exceptions;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;

namespace DulcisX.Nodes
{
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        public IVsBuildPropertyStorage VsBuildPropertyStorage
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (!(UnderlyingHierarchy is IVsBuildPropertyStorage vsBuildPropertyStorage))
                {
                    throw new InvalidHierarchyItemException($"This item does not support the '{nameof(IVsBuildPropertyStorage)}' interface.");
                }

                return vsBuildPropertyStorage;
            }
        }

        private IVsProject _underlyingProject;

        public IVsProject UnderlyingProject
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (_underlyingProject is null)
                {
                    _underlyingProject = (IVsProject)UnderlyingHierarchy;
                }

                return _underlyingProject;
            }
        }

        public override NodeTypes NodeType { get; }

        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy, NodeTypes nodeType = NodeTypes.Project) : base(solution, hierarchy, CommonNodeIds.Project)
        {
            NodeType = nodeType;
        }

        public Guid GetGuid()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentSolution.UnderlyingSolution.GetGuidOfProject(UnderlyingHierarchy, out var underlyingGuid);

            ErrorHandler.ThrowOnFailure(result);

            return underlyingGuid;
        }

        public string GetFileName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, CommonNodeIds.Project, (int)__VSHPROPID.VSHPROPID_Name);
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }

        public bool IsLoaded()
        {
            return !HierarchyUtilities.IsStubHierarchy(UnderlyingHierarchy);
        }

        public __VSPROJOUTPUTTYPE GetOutputTypeAction()
            => HierarchyUtilities.GetHierarchyProperty(UnderlyingHierarchy, ItemId, (int)__VSHPROPID5.VSHPROPID_OutputType, obj => (__VSPROJOUTPUTTYPE)Unbox.AsInt32(obj));

        public void SetOutputTypeAction(__VSPROJOUTPUTTYPE outputType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingHierarchy.SetProperty(ItemId, (int)__VSHPROPID5.VSHPROPID_OutputType, (int)outputType);
            ErrorHandler.ThrowOnFailure(result);
        }

        public string GetItemProperty(uint itemId, DocumentProperty documentProperty)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = VsBuildPropertyStorage.GetItemAttribute(itemId, documentProperty.ToString(), out var val);

            ErrorHandler.ThrowOnFailure(result);

            return val;
        }

        public void SetItemProperty(uint itemId, DocumentProperty documentProperty, string value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = VsBuildPropertyStorage.SetItemAttribute(itemId, documentProperty.ToString(), value);

            ErrorHandler.ThrowOnFailure(result);
        }

        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            while (!VsHelper.IsItemIdNil(node))
            {
                yield return NodeFactory.GetProjectItemNode(ParentSolution, this, UnderlyingHierarchy, node);

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
        }

        public VSADDRESULT CreateDocument(string name)
            => CreateDocument(this.ItemId, name);

        public VSADDRESULT CreateDocument(FolderNode parentNode, string name)
            => CreateDocument(parentNode.ItemId, name);

        private VSADDRESULT CreateDocument(uint parentId, string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var addResult = new VSADDRESULT[1];

            var result = UnderlyingProject.AddItem(parentId, VSADDITEMOPERATION.VSADDITEMOP_CLONEFILE, name, 1, new string[] { Path.GetTempFileName() }, IntPtr.Zero, addResult);

            ErrorHandler.ThrowOnFailure(result);

            return addResult[0];
        }

        public VSADDRESULT AddExistingDocument(string fullName)
            => AddExistingDocument(this.ItemId, fullName);

        public VSADDRESULT AddExistingDocument(FolderNode parentNode, string fullName)
            => AddExistingDocument(parentNode.ItemId, fullName);

        internal VSADDRESULT AddExistingDocument(uint parentId, string fullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!File.Exists(fullName))
            {
                throw new FileNotFoundException("The method couldn't find the specified file.", fullName);
            }

            VSADDRESULT[] addResult = new VSADDRESULT[1];

            var result = UnderlyingProject.AddItem(parentId, VSADDITEMOPERATION.VSADDITEMOP_CLONEFILE, Path.GetFileName(fullName), 1, new string[] { fullName }, IntPtr.Zero, addResult);

            ErrorHandler.ThrowOnFailure(result);

            return addResult[0];
        }

        public bool TryRemoveChildren(ProjectItemNode node, out int errorCode)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var project = (IVsProject2)UnderlyingProject;

            errorCode = project.RemoveItem(0, node.ItemId, out var success);

            return VsConverter.Boolean(success) && ErrorHandler.Succeeded(errorCode);
        }

        public bool TryGetPhysicalNode<TNode>(string fullName, out TNode node) where TNode : ProjectItemNode, IPhysicalNode
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var priority = new VSDOCUMENTPRIORITY[1];

            var result = UnderlyingProject.IsDocumentInProject(fullName, out var found, priority, out var item);

            ErrorHandler.ThrowOnFailure(result);

            if (!VsConverter.Boolean(found))
            {
                node = null;
                return false;
            }

            var baseNode = NodeFactory.GetProjectItemNode(ParentSolution, this, this.UnderlyingHierarchy, item);

            node = baseNode as TNode;
            return node is object;
        }

        internal uint GetDocumentCookie(DocumentNode document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingProject.GetMkDocument(document.ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return ((IVsRunningDocumentTable4)ParentSolution.RunningDocumentTable).GetDocumentCookie(fullName);
        }

        public void SaveAllChildren(bool forceSave = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentSolution.UnderlyingSolution.SaveSolutionElement(forceSave ? (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave : (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_SaveIfDirty, UnderlyingHierarchy, 0);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}
