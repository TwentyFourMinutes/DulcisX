using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy)
        {
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
