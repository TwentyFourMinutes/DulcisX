using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class SolutionNode : SolutionItemNode, IPhysicalNode
    {
        public override SolutionNode ParentSolution => this;

        public SolutionNode(IVsHierarchy hierarchy) : base(null, hierarchy)
        {
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
