using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using DulcisX.Core.Extensions;
using DulcisX.Core.Enums.VisualStudio;
using System.Runtime.InteropServices;

namespace DulcisX.Nodes
{
    public class SelectedNodes : IEnumerable<BaseNode>
    {
        private readonly SolutionNode _solution;

        private IVsMonitorSelection _monitorSelection;

        private IVsMonitorSelection MonitorSelection
            => _monitorSelection ?? (_monitorSelection = _solution.ServiceContainer.GetCOMInstance<IVsMonitorSelection>());

        internal SelectedNodes(SolutionNode solution)
            => _solution = solution;

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
                //Unknown ItemTypes
            }
        }

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

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
