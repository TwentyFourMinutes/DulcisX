using DulcisX.Core.Extensions;
using DulcisX.Core.Enums;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents a Node which couldn't be identified.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class UnknownNode : BaseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="UnknownNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy in which the <see cref="UnknownNode"/> sits in.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="UnknownNode"/> in the <paramref name="hierarchy"/>.</param>
        public UnknownNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }

        /// <inheritdoc/>
        /// <remarks>A <see cref="UnknownNode"/> doesn't support the iteration of any children.</remarks>
        public override IEnumerable<BaseNode> GetChildren()
        {
            throw new NotSupportedException("Iterating over Unknown Node children is not supported.");
        }

        /// <inheritdoc/>
        public override BaseNode GetParent()
        {
            if (ItemId == CommonNodeIds.Root)
            {
                if (!UnderlyingHierarchy.TryGetParentHierarchy(out var tempHierarchy))
                {
                    return null;
                }

                return NodeFactory.GetSolutionItemNode(ParentSolution, tempHierarchy, CommonNodeIds.Root);
            }
            else
            {
                var parentItemId = GetParentNodeId();

                if (parentItemId == CommonNodeIds.Nil)
                {
                    return null;
                }

                return NodeFactory.GetSolutionItemNode(ParentSolution, UnderlyingHierarchy, parentItemId);
            }
        }
    }
}
