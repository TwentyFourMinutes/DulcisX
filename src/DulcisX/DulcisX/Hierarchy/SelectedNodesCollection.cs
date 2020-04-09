using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using System.Runtime.InteropServices;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// A wrapper for the <see cref="IVsMonitorSelection"/>, gets access to the selected nodes in the Solution Explorer.
    /// </summary>
    public class SelectedNodesCollection : IEnumerable<BaseNode>
    {
        private readonly SolutionNode _solution;

        private IVsMonitorSelection _monitorSelection;

        private IVsMonitorSelection MonitorSelection
            => _monitorSelection ?? (_monitorSelection = _solution.ServiceContainer.GetCOMInstance<IVsMonitorSelection>());

        internal SelectedNodesCollection(SolutionNode solution)
            => _solution = solution;

        /// <inheritdoc/>
        public IEnumerator<BaseNode> GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = MonitorSelection.GetCurrentSelection(out var hierarchyPointer, out var itemId, out var multiSelect, out _);

            ErrorHandler.ThrowOnFailure(result);

            var hierarchy = (IVsHierarchy)Marshal.GetObjectForIUnknown(hierarchyPointer);

            Marshal.Release(hierarchyPointer);

            foreach (var selectedNode in GetSelection(multiSelect, hierarchy, itemId, _solution))
            {
                yield return selectedNode;
            }
        }

        /// <summary>
        /// Returns all selected nodes.
        /// </summary>
        /// <param name="multiSelect">The native <see cref="IVsMultiItemSelect"/> interface containg the selected Nodes.</param>
        /// <param name="hierarchy">The hierarchy which could contain the selected Node.</param>
        /// <param name="itemId">The potential Unique Identifier for the selected Node in the <paramref name="hierarchy"/>.</param>
        /// <param name="solution">The Solution in which the Nodes sit in.</param>
        /// <returns>An <see cref="IEnumerable{BaseNode}"/> with the selected Nodes.</returns>
        public static IEnumerable<BaseNode> GetSelection(IVsMultiItemSelect multiSelect, IVsHierarchy hierarchy, uint itemId, SolutionNode solution)
        {
            if (itemId == CommonNodeIds.MutlipleSelectedNodes)
            {
                foreach (var selectedNode in GetMultiSelection(multiSelect, solution))
                {
                    yield return selectedNode;
                }
            }
            else
            {
                yield return GetSingleSelection(hierarchy, itemId, solution);
            }
        }

        /// <summary>
        /// Returns the single selected node in the <see cref="IVsMultiItemSelect"/>.
        /// </summary>
        /// <param name="hierarchy">The hierarchy which contains the selected Node.</param>
        /// <param name="itemId">The Unique Identifier for the selected Node in the <paramref name="hierarchy"/>.</param>
        /// <param name="solution">The Solution in which the Nodes sit in.</param>
        /// <returns>An <see cref="IEnumerable{BaseNode}"/> with the selected Nodes.</returns>
        public static BaseNode GetSingleSelection(IVsHierarchy hierarchy, uint itemId, SolutionNode solution)
        {
            if (hierarchy is null)
            {
                return solution;
            }
            else if (itemId == CommonNodeIds.Root)
            {
                return NodeFactory.GetSolutionItemNode(solution, hierarchy, CommonNodeIds.Root);
            }
            else
            {
                return NodeFactory.GetProjectItemNode(solution, null, hierarchy, itemId);
            }
        }

        /// <summary>
        /// Returns all selected nodes is an <see cref="IVsMultiItemSelect"/> in the <paramref name="multiSelect"/>.
        /// </summary>
        /// <param name="multiSelect">The native <see cref="IVsMultiItemSelect"/> interface containg the selected Nodes.</param>
        /// <param name="solution">The Solution in which the Nodes sit in.</param>
        /// <returns>An <see cref="IEnumerable{BaseNode}"/> with the selected Nodes.</returns>
        public static IEnumerable<BaseNode> GetMultiSelection(IVsMultiItemSelect multiSelect, SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = multiSelect.GetSelectionInfo(out var selectionCount, out _);

            ErrorHandler.ThrowOnFailure(result);

            var itemSelection = new VSITEMSELECTION[selectionCount];

            result = multiSelect.GetSelectedItems(0u, selectionCount, itemSelection);

            ErrorHandler.ThrowOnFailure(result);

            for (int i = 0; i < itemSelection.Length; i++)
            {
                var item = itemSelection[i];

                yield return NodeFactory.GetItemNode(solution, item.pHier, item.itemid);
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
