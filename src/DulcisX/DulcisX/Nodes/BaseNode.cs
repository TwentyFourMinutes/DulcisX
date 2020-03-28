using DulcisX.Core.Extensions;
using DulcisX.Core.Enums;
using DulcisX.Exceptions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents the most basic Hierarchy Node.
    /// </summary>
    public abstract class BaseNode : INamedNode
    {
        /// <inheritdoc/>
        public virtual SolutionNode ParentSolution { get; }
        /// <inheritdoc/>
        public IVsHierarchy UnderlyingHierarchy { get; }

        /// <inheritdoc/>
        public uint ItemId { get; }

        /// <inheritdoc/>
        public abstract NodeTypes NodeType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the Node sits in.</param>
        /// <param name="hierarchy">The Hierarchy in which the Node sits in.<param>
        /// <param name="itemId">The Unique Identifier for the Node in the <paramref name="hierarchy"/>.</param>
        protected BaseNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId)
        {
            ParentSolution = solution;
            UnderlyingHierarchy = hierarchy;
            ItemId = itemId;
        }

        /// <inheritdoc/>
        public string GetDisplayName()
            => AsHierarchyItem().Text;

        /// <inheritdoc/>
        public abstract BaseNode GetParent();

        /// <inheritdoc/>
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

        internal uint GetParentNodeId()
            => UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_Parent);

        /// <inheritdoc/>
        public IVsHierarchyItem AsHierarchyItem()
        {
            var manager = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemManager>();

            return manager.GetHierarchyItem(UnderlyingHierarchy, ItemId);
        }

        /// <summary>
        /// Returns all immediate children Nodes of the current Node.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{BaseNode}"/> with the children.</returns>
        public abstract IEnumerable<BaseNode> GetChildren();

        /// <inheritdoc/>
        public bool IsTypeMatching(NodeTypes nodeType)
            => NodeType == nodeType;
    }
}
