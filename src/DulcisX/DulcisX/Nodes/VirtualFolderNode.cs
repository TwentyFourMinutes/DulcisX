using DulcisX.Components;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public class VirtualFolderNode : SolutionItemNode
    {
        public override string Name => throw new NotImplementedException();

        public VirtualFolderNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy)
        {
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
