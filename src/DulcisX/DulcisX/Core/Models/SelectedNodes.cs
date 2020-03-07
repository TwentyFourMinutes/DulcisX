using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using DulcisX.Nodes;
using DulcisX.Core.Extensions;

namespace DulcisX.Core.Models
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

            var result = MonitorSelection.GetCurrentSelection(out _, out _, out var selection, out _);

            if (selection is null)
                yield break;

            ErrorHandler.ThrowOnFailure(result);

            result = selection.GetSelectionInfo(out var selectionCount, out _);

            ErrorHandler.ThrowOnFailure(result);

            var itemSelection = new VSITEMSELECTION[selectionCount];

            result = selection.GetSelectedItems(0u, selectionCount, itemSelection);

            ErrorHandler.ThrowOnFailure(result);

            for (int i = 0; i < itemSelection.Length; i++)
            {
                var item = itemSelection[i];

                yield return NodeFactory.GetItemNode(_solution, item.pHier, item.itemid);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
