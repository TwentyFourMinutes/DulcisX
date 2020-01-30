using DulcisX.Components;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio.Shell;
using DulcisX.Helpers;
using static Microsoft.VisualStudio.VSConstants;

namespace DulcisX.Core.Models
{
    public class SelectedHierarchyItemsX : IEnumerable<SelectedItemX>
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

        public IEnumerator<SelectedItemX> GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = MonitorSelection.GetCurrentSelection(out _, out var itemId, out var selection, out _);
  
            VsHelper.ValidateVSStatusCode(result);

            result = selection.GetSelectionInfo(out var selectionCount, out var areAcrossHirarchies);

            VsHelper.ValidateVSStatusCode(result);

            var itemSelection = new VSITEMSELECTION[selectionCount];

            result = selection.GetSelectedItems(0u, selectionCount, itemSelection);

            VsHelper.ValidateVSStatusCode(result);

            for (int i = 0; i < itemSelection.Length; i++)
            {
                var item = itemSelection[i];

                yield return new SelectedItemX(item.pHier, item.itemid);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
