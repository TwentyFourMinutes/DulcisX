using System;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Components;
using System.Linq;
using DulcisX.Core;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using DulcisX.Core.Models.Interfaces;
using Microsoft.VisualStudio.OLE.Interop;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.ProjectSystem.VS;

namespace DulcisX.TestVSIX
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(DulcisXTestVSIXPackage.PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class DulcisXTestVSIXPackage : PackageX
    {
        public const string PackageGuidString = "a7c50965-01fc-4668-9b93-c14bad2dbe25";

        #region Package Members

        public DulcisXTestVSIXPackage()
        {
            base.OnInitializeAsync += OnInitAsync;
        }

        private async Task OnInitAsync(CancellationToken arg2, IProgress<ServiceProgressData> arg1)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(arg2);

            Solution.DocumentEvents.OnSave += DocumentEvents_OnSave;
        }

        private void DocumentEvents_OnSave(HierarchyItemX obj)
        {

        }

        #endregion
    }
}