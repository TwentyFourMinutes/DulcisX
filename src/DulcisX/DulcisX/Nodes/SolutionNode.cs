using DulcisX.Components;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public class SolutionNode : SolutionItemNode, IPhysicalNode
    {
        public string FullName => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override SolutionNode ParentSolution => this;

        public SolutionNode(IVsHierarchy hierarchy) : base(null, hierarchy)
        {
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
