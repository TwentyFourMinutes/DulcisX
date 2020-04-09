using System;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents an <see cref="IPhysicalNode"/> which Source Control state changed.
    /// </summary>
    /// <typeparam name="TNodeType">The type of the node which got changed.</typeparam>
    /// <typeparam name="TFlag">The enumeration which specifies what happend to the <see cref="IPhysicalNode"/>.</typeparam>
    public class ChangedPhysicalSccNode<TNodeType, TFlag> : ChangedNodeBase<TNodeType, TFlag>
                                                           where TNodeType : IPhysicalNode
                                                           where TFlag : struct, Enum
    {
        internal ChangedPhysicalSccNode(TNodeType node, TFlag flag) : base(node, flag)
        {
        }
    }
}
