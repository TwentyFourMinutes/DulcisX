using System;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a changed <see cref="INamedNode"/>.
    /// </summary>
    /// <typeparam name="TNodeType">The type of the node which got changed.</typeparam>
    /// <typeparam name="TFlag">The enumeration which specifies what happend to the <see cref="INamedNode"/>.</typeparam>
    public abstract class ChangedNodeBase<TNodeType, TFlag> where TNodeType : INamedNode
                                                            where TFlag : struct, Enum
    {
        /// <summary>
        /// Gets the node which got changed.
        /// </summary>
        public TNodeType Node { get; }

        /// <summary>
        /// Gets the enumeration which specifies what happend to the <see cref="INamedNode"/>.
        /// </summary>
        public TFlag Flag { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangedNodeBase{TNodeType, TFlag}"/> class.
        /// </summary>
        /// <param name="node">The Node which got changed.</param>
        /// <param name="flag">The enumeration which specifies what happend.</param>
        protected ChangedNodeBase(TNodeType node, TFlag flag)
        {
            Node = node;
            Flag = flag;
        }
    }
}
