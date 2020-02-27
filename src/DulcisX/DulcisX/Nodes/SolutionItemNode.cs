using DulcisX.Components;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public abstract class SolutionItemNode : INamedNode, IEnumerable<SolutionItemNode>
    {
        public abstract string Name { get; }

        public virtual SolutionNode ParentSolution { get; }

        public IVsHierarchy UnderlyingHierarchy { get; }

        protected SolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy)
        {
            ParentSolution = solution;
            UnderlyingHierarchy = hierarchy;
        }

        public abstract IEnumerator<SolutionItemNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
