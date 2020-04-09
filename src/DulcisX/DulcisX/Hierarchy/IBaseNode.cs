using DulcisX.Core.Enums;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents the most basic Hierarchy Node.
    /// </summary>
    public interface IBaseNode : IEquatable<IBaseNode>
    {
        /// <summary>
        /// Gets the Solution in which the Node sits in.
        /// </summary>
        SolutionNode ParentSolution { get; }

        /// <summary>
        /// Gets the Hierarchy in which the Node sits in.
        /// </summary>
        IVsHierarchy UnderlyingHierarchy { get; }

        /// <summary>
        /// Gets the Unique Identifier for the Node in the <see cref="UnderlyingHierarchy"/>.
        /// </summary>
        uint ItemId { get; }

        /// <summary>
        /// Returns the immediate parent Node of the current Node.
        /// </summary>
        /// <returns>The parent if any could be found, otherwise null.</returns>
        BaseNode GetParent();

        /// <summary>
        /// Returns the first parent Node of the current Node, matching the given <paramref name="nodeType"/>.
        /// </summary>
        /// <param name="nodeType">The Node type which should be serached for.</param>
        /// <returns>The matching parent if any could be found, otherwise null.</returns>
        BaseNode GetParent(NodeTypes nodeType);

        /// <summary>
        /// Returns the <see cref="IVsHierarchyItem"/> which represents the current Node.
        /// </summary>
        /// <returns>The the matching <see cref="IVsHierarchyItem"/>.</returns>
        IVsHierarchyItem AsHierarchyItem();

        /// <summary>
        /// Returns all immediate children Nodes of the current Node.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{BaseNode}"/> with the children.</returns>
        IEnumerable<BaseNode> GetChildren();
    }
}
