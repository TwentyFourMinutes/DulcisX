using DulcisX.Core.Extensions;
using DulcisX.Core.Enums;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using System.Diagnostics;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents the most basic <see cref="SolutionNode"/> children Node.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class SolutionItemNode : BaseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionItemNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="SolutionItemNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the <see cref="SolutionItemNode"/> itself.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="SolutionItemNode"/> in the <paramref name="hierarchy"/>.</param>
        protected SolutionItemNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {

        }

        /// <inheritdoc/>
        public override BaseNode GetParent()
        {
            if (!UnderlyingHierarchy.TryGetParentHierarchy(out var parentHierarchy))
            {
                return null;
            }

            return NodeFactory.GetSolutionItemNode(ParentSolution, parentHierarchy, CommonNodeIds.Root);
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            while (!VsHelper.IsItemIdNil(node))
            {
                if (UnderlyingHierarchy.TryGetNestedHierarchy(node, out var nestedHierarchy))
                {
                    var child = NodeFactory.GetSolutionItemNode(ParentSolution, nestedHierarchy, CommonNodeIds.Root);

                    if (!(child is UnknownNode))
                    {
                        yield return child;
                    }
                }

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
        }

        /// <summary>
        /// Asynchronously gets a flat list of nodes that exist within the current node.
        /// </summary>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous request.</param>
        /// <returns> A task that when complete provides the flattened set of hierarchy items.</returns>
        /// <remarks> This method will never return, if being called in an <see cref="Core.PackageX.OnInitializeAsync"/> callback. You should instead wrap it in an <see cref="Microsoft.VisualStudio.Threading.JoinableTaskFactory.RunAsync(Func{System.Threading.Tasks.Task})"/> call.</remarks>
        public async Task<IEnumerable<BaseNode>> GetAllChildrenAsync(CancellationToken ct = default)
        {
            var collectionProvider = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemCollectionProvider>();

            return (await collectionProvider.GetDescendantsAsync(UnderlyingHierarchy, ct))
                                            .Select(hierarchyItem => NodeFactory.GetItemNode(ParentSolution, hierarchyItem))
                                            .Where(x=> !(x is UnknownNode));
        }

        /// <summary>
        /// Asynchronously gets a flat list of filtered nodes that exist within the current node.
        /// </summary>
        /// <param name="predicate">A function to test each node for a condition.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous request.</param>
        /// <returns> A task that when complete provides the flattened set of hierarchy items.</returns>
        /// <remarks> This method will never return, if being called in an <see cref="Core.PackageX.OnInitializeAsync"/> callback. You should instead wrap it in an <see cref="Microsoft.VisualStudio.Threading.JoinableTaskFactory.RunAsync(Func{System.Threading.Tasks.Task})"/> call.</remarks>
        public async Task<IEnumerable<BaseNode>> GetAllChildrenAsync(Predicate<BaseNode> predicate, CancellationToken cancellationToken = default)
        {
            var collectionProvider = ParentSolution.ServiceContainer.GetInstance<IVsHierarchyItemCollectionProvider>();

            var hierarchyItems = await collectionProvider.GetDescendantsAsync(UnderlyingHierarchy, cancellationToken);

            var filteredItems = await collectionProvider.GetFilteredHierarchyItemsAsync(hierarchyItems, hierarchyItem => predicate(NodeFactory.GetItemNode(ParentSolution, hierarchyItem)), cancellationToken);

            var filteredNodes = filteredItems.Select(hierarchyItem => NodeFactory.GetItemNode(ParentSolution, hierarchyItem))
                                             .Where(x => !(x is UnknownNode)); ;

            filteredItems.Dispose();

            return filteredNodes;
        }

        /// <summary>
        /// Saves the files and all children within the current <see cref="SolutionItemNode"/>.
        /// </summary>
        /// <param name="forceSave">Determines whether to force the file save operation or not.</param>
        public virtual void SaveAllChildren(bool forceSave = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentSolution.UnderlyingSolution.SaveSolutionElement(forceSave ? (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave : (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_SaveIfDirty, UnderlyingHierarchy, 0);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}