using DulcisX.Core.Extensions;
using DulcisX.Core.Enums.VisualStudio;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DulcisX.Nodes
{
    public abstract class SolutionItemNode : BaseNode
    {
        protected SolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        public override BaseNode GetParent()
        {
            if (!UnderlyingHierarchy.TryGetParentHierarchy(out var parentHierarchy))
            {
                return null;
            }

            return NodeFactory.GetSolutionItemNode(ParentSolution, parentHierarchy, CommonNodeIds.Root);
        }

        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            while (!VsHelper.IsItemIdNil(node))
            {
                if (UnderlyingHierarchy.TryGetNestedHierarchy(node, out var nestedHierarchy))
                {
                    yield return NodeFactory.GetSolutionItemNode(ParentSolution, nestedHierarchy, CommonNodeIds.Root);
                }

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
        }

        public async Task<IEnumerable<BaseNode>> GetAllChildrenAsync(CancellationToken ct = default)
        {
            var collectionProvider = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemCollectionProvider>();

            return (await collectionProvider.GetDescendantsAsync(UnderlyingHierarchy, ct).ConfigureAwait(false))
                                            .Select(hierarchyItem => NodeFactory.GetItemNode(ParentSolution, hierarchyItem));
        }

        public async Task<IEnumerable<BaseNode>> GetAllChildrenAsync(Predicate<BaseNode> predicate, CancellationToken ct = default)
        {
            var collectionProvider = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemCollectionProvider>();

            var hierarchyItems = await collectionProvider.GetDescendantsAsync(UnderlyingHierarchy, ct).ConfigureAwait(false);

            var filteredItems = await collectionProvider.GetFilteredHierarchyItemsAsync(hierarchyItems, hierarchyItem => predicate(NodeFactory.GetItemNode(ParentSolution, hierarchyItem)), ct).ConfigureAwait(false);

            var filteredNodes = filteredItems.Select(hierarchyItem => NodeFactory.GetItemNode(ParentSolution, hierarchyItem));

            filteredItems.Dispose();

            return filteredNodes;
        }
    }
}