using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class SolutionItemNode : INamedNode, IEnumerable<SolutionItemNode>
    {
        public virtual SolutionNode ParentSolution { get; }

        public IVsHierarchy UnderlyingHierarchy { get; }

        protected SolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy)
        {
            ParentSolution = solution;
            UnderlyingHierarchy = hierarchy;
        }

        public abstract string GetName();

        public abstract IEnumerator<SolutionItemNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
