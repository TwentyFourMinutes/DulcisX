using System;

namespace DulcisX.Nodes
{
    public class AddedPhysicalNode<TNodeType, TFlag> : ChangedNodeBase<TNodeType, TFlag>
                                                       where TNodeType : IPhysicalNode
                                                       where TFlag : struct, Enum
    {
        internal AddedPhysicalNode(TNodeType node, TFlag flag) : base(node, flag)
        {
        }
    }
}
