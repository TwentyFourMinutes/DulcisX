using System;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents a renamed <see cref="IPhysicalNode"/>.
    /// </summary>
    /// <typeparam name="TNodeType">The type of the node which got changed.</typeparam>
    /// <typeparam name="TFlag">The enumeration which specifies what happend to the <see cref="IPhysicalNode"/>.</typeparam>
    public class RenamedPhysicalNode<TNodeType, TFlag> : ChangedNodeBase<TNodeType, TFlag>
                                                         where TNodeType : IPhysicalNode
                                                         where TFlag : struct, Enum
    {
        /// <summary>
        /// Gets the old full name of the renamed <see cref="IPhysicalNode"/>.
        /// </summary>
        public string OldFullName { get; }

        /// <summary>
        /// Gets the new full name of the renamed <see cref="IPhysicalNode"/>.
        /// </summary>
        public string NewFullName { get; }

        internal RenamedPhysicalNode(TNodeType node, string oldFullName, string newFullName, TFlag flag) : base(node, flag)
        {
            OldFullName = oldFullName;
            NewFullName = newFullName;
        }
    }
}
