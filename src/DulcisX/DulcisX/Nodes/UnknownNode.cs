﻿using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public class UnknownNode : BaseNode
    {
        public override NodeTypes NodeType => NodeTypes.Unknown;

        public UnknownNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }

        public override IEnumerable<BaseNode> GetChildren()
        {
            throw new NotSupportedException("Iterating over Unknown Node children is not supported.");
        }

        public override BaseNode GetParent()
        {
            if (ItemId == CommonNodeId.Root)
            {
                if (!UnderlyingHierarchy.TryGetParentHierarchy(out var tempHierarchy))
                {
                    return null;
                }

                return NodeFactory.GetSolutionItemNode(ParentSolution, tempHierarchy, CommonNodeId.Root);
            }
            else
            {
                var parentItemId = UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_Parent);

                if (parentItemId == CommonNodeId.Nil)
                {
                    return null;
                }

                return NodeFactory.GetSolutionItemNode(ParentSolution, UnderlyingHierarchy, parentItemId);
            }
        }
    }
}
