using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Components;
using DulcisX.Core;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace DulcisX.TestVSIX
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class DulcisXTestVSIXPackage : PackageX
    {
        public const string PackageGuidString = "a7c50965-01fc-4668-9b93-c14bad2dbe25";

        #region Package Members

        public DulcisXTestVSIXPackage()
        {
            OnInitializeAsync += OnInitAsync;
        }

        private async Task OnInitAsync(CancellationToken arg2, IProgress<ServiceProgressData> arg1)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(arg2);

            Solution.HierarchyItemEvents.OnSave += DocumentEvents_OnSave;
            Solution.HierarchyItemEvents.OnAttributeChanged += DocumentEvents_OnAttributeChanged;
            Solution.HierarchyItemEvents.OnDocumentLocked += DocumentEvents_OnDocumentLocked;
            Solution.HierarchyItemEvents.OnDocumentUnlocked += DocumentEvents_OnDocumentUnlocked;
            Solution.HierarchyItemEvents.OnDocumentWindowHidden += DocumentEvents_OnDocumentWindowHidden;
            Solution.HierarchyItemEvents.OnDocumentWindowShow += DocumentEvents_OnDocumentWindowShow;
            Solution.HierarchyItemEvents.OnSaved += DocumentEvents_OnSaved;
        }

        private void DocumentEvents_OnSaved(HierarchyItemX obj)
        {

        }

        private void DocumentEvents_OnDocumentWindowShow(HierarchyItemX arg1, bool arg2, Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame arg3)
        {

        }

        private void DocumentEvents_OnDocumentWindowHidden(HierarchyItemX arg1, Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame arg2)
        {

        }

        private void DocumentEvents_OnDocumentUnlocked(HierarchyItemX arg1, Microsoft.VisualStudio.Shell.Interop._VSRDTFLAGS arg2, uint arg3, uint arg4)
        {

        }

        private void DocumentEvents_OnDocumentLocked(HierarchyItemX arg1, Microsoft.VisualStudio.Shell.Interop._VSRDTFLAGS arg2, uint arg3, uint arg4)
        {

        }

        private void DocumentEvents_OnAttributeChanged(HierarchyItemX arg1, VsRDTAttributeX arg2)
        {

        }

        private void DocumentEvents_OnSave(HierarchyItemX obj)
        {

        }

        #endregion
    }
}