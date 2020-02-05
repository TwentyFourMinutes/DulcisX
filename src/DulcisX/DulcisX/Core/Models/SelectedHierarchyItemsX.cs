using DulcisX.Components;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio.Shell;
using DulcisX.Helpers;

namespace DulcisX.Core.Models
{
    public class SelectedHierarchyItemsX : IEnumerable<HierarchyItemX>
    {
        private IVsMonitorSelection _monitorSelection;

        private IVsMonitorSelection MonitorSelection
        {
            get
            {
                if (_monitorSelection is null)
                {
                    _monitorSelection = _solution.ServiceProviders.GetService<SVsShellMonitorSelection, IVsMonitorSelection>();
                }

                return _monitorSelection;
            }
        }

        private readonly SolutionX _solution;

        internal SelectedHierarchyItemsX(SolutionX solution)
            => _solution = solution;

        public IEnumerator<HierarchyItemX> GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = MonitorSelection.GetCurrentSelection(out _, out _, out var selection, out _);

            VsHelper.ValidateSuccessStatusCode(result);

            result = selection.GetSelectionInfo(out var selectionCount, out _);

            VsHelper.ValidateSuccessStatusCode(result);

            var itemSelection = new VSITEMSELECTION[selectionCount];

            result = selection.GetSelectedItems(0u, selectionCount, itemSelection);

            VsHelper.ValidateSuccessStatusCode(result);

            for (int i = 0; i < itemSelection.Length; i++)
            {
                var item = itemSelection[i];

                var type = item.pHier.GetHierarchyItemType(item.itemid);

                yield return new HierarchyItemX(item.pHier, item.itemid, type, ConstructorInstance.FromValue(_solution), ConstructorInstance.Empty<ProjectX>());
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
