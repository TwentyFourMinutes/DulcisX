using System;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a newly added <see cref="IPhysicalNode"/>.
    /// </summary>
    /// <typeparam name="TNodeType">The type of the node which got changed.</typeparam>
    /// <typeparam name="TFlag">The enumeration which specifies what happend to the <see cref="IPhysicalNode"/>.</typeparam>
    public class AddedPhysicalNode<TNodeType, TFlag> : ChangedNodeBase<TNodeType, TFlag>
                                                       where TNodeType : IPhysicalNode
                                                       where TFlag : struct, Enum
    {
        internal AddedPhysicalNode(TNodeType node, TFlag flag) : base(node, flag)
        {
        }
    }
}
