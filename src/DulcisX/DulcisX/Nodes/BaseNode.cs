using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Exceptions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class BaseNode : INamedNode
    {
        public virtual SolutionNode ParentSolution { get; }

        public IVsHierarchy UnderlyingHierarchy { get; }

        public uint ItemId { get; }

        public abstract NodeTypes NodeType { get; }

        protected BaseNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            ParentSolution = solution;
            UnderlyingHierarchy = hierarchy;
            ItemId = itemId;
        }

        public string GetDisplayName()
            => AsHierarchyItem().Text;

        public abstract BaseNode GetParent();

        public virtual BaseNode GetParent(NodeTypes nodeType)
        {
            if (nodeType.ContainsMultipleFlags())
            {
                throw new NoFlagsAllowedException(nameof(NodeTypes));
            }

            else if (nodeType == NodeTypes.Solution)
            {
                return ParentSolution;
            }

            BaseNode parent = this.GetParent();

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

        public abstract IEnumerable<BaseNode> GetChildren();

        public bool IsTypeMatching(NodeTypes nodeType)
            => NodeType == nodeType;
    }
}
