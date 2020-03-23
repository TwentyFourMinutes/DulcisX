using System;

namespace DulcisX.Nodes
{
    public abstract class ChangedNodeBase<TNodeType, TFlag> where TNodeType : INamedNode
                                                            where TFlag : struct, Enum
    {
        public TNodeType Node { get; }

        public TFlag Flag { get; }

        protected ChangedNodeBase(TNodeType node, TFlag flag)
        {
            Node = node;
            Flag = flag;
        }
    }
}
