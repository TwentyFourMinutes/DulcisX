using DulcisX.Components;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        public string FullName => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        private IVsProject myVar;

        public IVsProject MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }


        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy)
        {
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
