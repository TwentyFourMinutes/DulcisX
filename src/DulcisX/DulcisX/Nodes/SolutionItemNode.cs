using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public abstract class SolutionItemNode : ItemNode
    {
        protected SolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override ItemNode GetParent()
        {
            if (!UnderlyingHierarchy.TryGetParentHierarchy(out var parentHierarchy))
            {
                return null;
            }

            return NodeFactory.GetSolutionItemNode(ParentSolution, parentHierarchy, CommonNodeId.Root);
        }

        public override IEnumerator<ItemNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}