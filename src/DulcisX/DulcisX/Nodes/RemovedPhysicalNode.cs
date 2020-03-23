using System;

namespace DulcisX.Nodes
{
    public class RemovedPhysicalNode<TFlag> where TFlag : struct, Enum
    {
        public ProjectNode ParentProject { get; }

        public string FullName { get; }

        public TFlag Flag { get; }

        internal RemovedPhysicalNode(ProjectNode project, string fullName, TFlag flag)
        {
            ParentProject = project;
            FullName = fullName;
            Flag = flag;
        }
    }
}
