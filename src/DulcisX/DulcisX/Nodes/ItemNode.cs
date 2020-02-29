using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Exceptions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class ItemNode : INamedNode, IEnumerable<ItemNode>
    {
        public SolutionNode ParentSolution { get; }

        public IVsHierarchy UnderlyingHierarchy { get; }

        public uint ItemId { get; }

        public abstract NodeTypes NodeType { get; }

        protected ItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            ParentSolution = solution;
            UnderlyingHierarchy = hierarchy;
            ItemId = itemId;
        }

        public abstract string GetName();

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

        public IVsHierarchyItem GetHierarchyItem()
        {
            var manager = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            return manager.GetHierarchyItem(UnderlyingHierarchy, ItemId);
        }

        public bool IsTypeMatching(NodeTypes nodeType)
            => NodeType == nodeType;

        public abstract IEnumerator<ItemNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
