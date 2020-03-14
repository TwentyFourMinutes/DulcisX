using DulcisX.Core.Models.Enums;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public interface IBaseNode
    {
        SolutionNode ParentSolution { get; }

        IVsHierarchy UnderlyingHierarchy { get; }

        uint ItemId { get; }

        NodeTypes NodeType { get; }

        BaseNode GetParent();

        BaseNode GetParent(NodeTypes nodeType);

        IVsHierarchyItem AsHierarchyItem();

        IEnumerable<BaseNode> GetChildren();

        bool IsTypeMatching(NodeTypes nodeType);
    }
}
