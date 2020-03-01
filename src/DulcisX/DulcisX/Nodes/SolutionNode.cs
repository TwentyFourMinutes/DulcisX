using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;
using System.Collections.Generic;

namespace DulcisX.Nodes
{
    public class SolutionNode : BaseNode, IPhysicalNode
    {
        public IVsSolution UnderlyingSolution { get; }

        public override NodeTypes NodeType => NodeTypes.Solution;

        public override SolutionNode ParentSolution => this;

        public Container ServiceContainer { get; }

        public SolutionNode(IVsSolution solution, Container container) : base(null, (IVsHierarchy)solution, CommonNodeIds.Solution)
        {
            UnderlyingSolution = solution;
            ServiceContainer = container;
        }

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return (string)fullName;
        }

        public override BaseNode GetParent()
            => null;

        public override BaseNode GetParent(NodeTypes nodeType)
            => null;

        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            do
            {
                if (VsHelper.IsItemIdNil(node))
                {
                    yield break;
                }

                if (UnderlyingHierarchy.TryGetNestedHierarchy(node, out var nestedHierarchy))
                {
                    yield return NodeFactory.GetSolutionItemNode(ParentSolution, nestedHierarchy, CommonNodeIds.Root);
                }

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
            while (true);
        }
    }
}
