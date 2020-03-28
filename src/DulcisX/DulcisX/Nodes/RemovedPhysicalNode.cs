using System;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a removed <see cref="IPhysicalNode"/>.
    /// </summary>
    /// <typeparam name="TFlag">The enumeration which specifies what happend to the <see cref="IPhysicalNode"/>.</typeparam>
    public class RemovedPhysicalNode<TFlag> where TFlag : struct, Enum
    {
        /// <summary>
        /// Gets the project in which the <see cref="IPhysicalNode"/> was.
        /// </summary>
        public ProjectNode ParentProject { get; }

        /// <summary>
        /// Gets a string which contains the full name of the removed <see cref="IPhysicalNode"/>.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the enumeration which specifies what happend to the <see cref="IPhysicalNode"/>.
        /// </summary>
        public TFlag Flag { get; }

        internal RemovedPhysicalNode(ProjectNode project, string fullName, TFlag flag)
        {
            ParentProject = project;
            FullName = fullName;
            Flag = flag;
        }
    }
}
