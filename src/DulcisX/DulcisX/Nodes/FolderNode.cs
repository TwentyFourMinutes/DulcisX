using DulcisX.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public class FolderNode : ProjectItemNode, IPhysicalNode
    {
        public string FullName => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public FolderNode(SolutionNode solution, ProjectX project) : base(solution, project)
        {
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
