using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Exceptions;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

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

        public IVsProject UnderlyingProject => (IVsProject)UnderlyingHierarchy;

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

        public __VSPROJOUTPUTTYPE GeOutputTypeAction()
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

        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            do
            {
                if (VsHelper.IsItemIdNil(node))
                {
                    yield break;
                }

                yield return NodeFactory.GetProjectItemNode(ParentSolution, this, UnderlyingHierarchy, node);

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
            while (true);
        }
    }
}
