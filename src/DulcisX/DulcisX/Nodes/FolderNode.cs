using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class FolderNode : ProjectItemNode, IPhysicalNode
    {
        public FolderNode(SolutionNode solution, ProjectNode project) : base(solution, project)
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

        public override ProjectNode GetParentProject()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<SolutionItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
