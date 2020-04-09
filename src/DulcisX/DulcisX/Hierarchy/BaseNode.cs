using DulcisX.Core.Extensions;
using DulcisX.Core.Enums;
using DulcisX.Exceptions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Hierarchy
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the Node sits in.</param>
        /// <param name="hierarchy">The Hierarchy in which the Node sits in.</param>
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

            while (!parent.IsTypeMatching(nodeType) && parent is object)
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

        #region Equality Comparison

        /// <inheritdoc/>
        public static bool operator ==(BaseNode node1, BaseNode node2)
            => Compare(node1, node2);

        /// <inheritdoc/>
        public static bool operator ==(IBaseNode node1, BaseNode node2)
            => Compare(node1, node2);

        /// <inheritdoc/>
        public static bool operator ==(BaseNode node1, IBaseNode node2)
            => Compare(node1, node2);

        /// <inheritdoc/>
        public static bool operator !=(BaseNode node1, BaseNode node2)
            => !Compare(node1, node2);

        /// <inheritdoc/>
        public static bool operator !=(IBaseNode node1, BaseNode node2)
            => !Compare(node1, node2);

        /// <inheritdoc/>
        public static bool operator !=(BaseNode node1, IBaseNode node2)
             => !Compare(node1, node2);

        private static bool Compare(IBaseNode node1, IBaseNode node2)
        {
            if (ReferenceEquals(node1, node2))
            {
                return true;
            }

            if (ReferenceEquals(node1, null) ||
                ReferenceEquals(node2, null))
            {
                return false;
            }

            return node1.ItemId.Equals(node1.ItemId)
                   && node1.UnderlyingHierarchy.Equals(node2.UnderlyingHierarchy);
        }

        /// <inheritdoc/>
        public virtual bool Equals(IBaseNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ItemId.Equals(other.ItemId)
                   && UnderlyingHierarchy.Equals(other.UnderlyingHierarchy);
        }

        /// <inheritdoc/>
        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((BaseNode)obj);
        }

        /// <inheritdoc/>
        public sealed override int GetHashCode()
        {
            var hashCode1 = UnderlyingHierarchy.GetHashCode();
            var hashCode2 = ItemId.GetHashCode();

            return ((hashCode1 << 5) + hashCode1) ^ hashCode2;
        }

        #endregion
    }
}