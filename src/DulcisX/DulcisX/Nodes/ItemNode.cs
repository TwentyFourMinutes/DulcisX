using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Exceptions;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class ItemNode : INamedNode, IEnumerable<ItemNode>
    {
        public virtual SolutionNode ParentSolution { get; }

        public IVsHierarchy UnderlyingHierarchy { get; }

        public uint ItemId { get; }

        public abstract NodeTypes NodeType { get; }

        protected ItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            ParentSolution = solution;
            UnderlyingHierarchy = hierarchy;
            ItemId = itemId;
        }

        public string GetName()
            => AsHierarchyItem().Text;

        public abstract ItemNode GetParent();

        public virtual ItemNode GetParent(NodeTypes nodeType)
        {
            if (nodeType.ContainsMultipleFlags())
            {
                throw new NoFlagsAllowedException(nameof(NodeTypes));
            }

            else if (nodeType == NodeTypes.Solution)
            {
                return ParentSolution;
            }

            ItemNode parent = this.GetParent();

            while (parent.IsTypeMatching(nodeType))
            {
                parent = parent.GetParent();
            }

            return parent;
        }

        public IVsHierarchyItem AsHierarchyItem()
        {
            var manager = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            return manager.GetHierarchyItem(UnderlyingHierarchy, ItemId);
        }

        public bool IsTypeMatching(NodeTypes nodeType)
            => NodeType == nodeType;

        public IEnumerator<ItemNode> GetEnumerator()
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
                    yield return NodeFactory.GetItemNode(ParentSolution, nestedHierarchy, CommonNodeId.Root);
                }

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);

                node = UnderlyingHierarchy.GetProperty(node, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling);
            }
            while (true);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
