using System;

namespace DulcisX.Nodes
{
    public class ChangedPhysicalSccNode<TNodeType, TFlag> : ChangedNodeBase<TNodeType, TFlag>
                                                           where TNodeType : IPhysicalNode
                                                           where TFlag : struct, Enum
    {
        internal ChangedPhysicalSccNode(TNodeType node, TFlag flag) : base(node, flag)
        {
        }
    }
}
