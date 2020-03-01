using DulcisX.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class FolderNode : ProjectItemNode, IPhysicalNode
    {
        public override NodeTypes NodeType => NodeTypes.Folder;

        public FolderNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project, itemId)
        {
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
