using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class SolutionItemNode : BaseNode
    {
        protected SolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        public override BaseNode GetParent()
        {
            if (!UnderlyingHierarchy.TryGetParentHierarchy(out var parentHierarchy))
            {
                return null;
            }

            return NodeFactory.GetSolutionItemNode(ParentSolution, parentHierarchy, CommonNodeId.Root);
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

                if (UnderlyingHierarchy.TryGetNestedHierarchy(node, out var nestedHierarchy))
                {
                    yield return NodeFactory.GetSolutionItemNode(ParentSolution, nestedHierarchy, CommonNodeId.Root);
                }

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
            while (true);
        }
    }
}