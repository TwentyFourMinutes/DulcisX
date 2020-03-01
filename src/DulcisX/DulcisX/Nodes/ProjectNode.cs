using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        public IVsProject UnderlyingProject => (IVsProject)UnderlyingHierarchy;

        public override NodeTypes NodeType { get; }

        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy, NodeTypes nodeType = NodeTypes.Project) : base(solution, hierarchy, CommonNodeId.Project)
        {
            NodeType = nodeType;
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
