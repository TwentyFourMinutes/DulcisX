using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class VirtualFolderNode : SolutionItemNode
    {
        public VirtualFolderNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy)
        {
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
