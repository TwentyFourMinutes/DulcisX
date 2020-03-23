using System;

namespace DulcisX.Nodes
{
    public class RenamedPhysicalNode<TNodeType, TFlag> : ChangedNodeBase<TNodeType, TFlag>
                                                         where TNodeType : IPhysicalNode
                                                         where TFlag : struct, Enum
    {
        public string OldFullName { get; }

        public string NewFullName { get; }

        internal RenamedPhysicalNode(TNodeType node, string oldFullName, string newFullName, TFlag flag) : base(node, flag)
        {
            OldFullName = oldFullName;
            NewFullName = newFullName;
        }
    }
}
