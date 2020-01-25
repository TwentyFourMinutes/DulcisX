using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Core;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace DulcisX.TestVSIX
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(DulcisXTestVSIXPackage.PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class DulcisXTestVSIXPackage : PackageX
    {
        /// <summary>
        /// DulcisX.TestVSIXPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a7c50965-01fc-4668-9b93-c14bad2dbe25";

        #region Package Members

        public DulcisXTestVSIXPackage()
        {
            base.OnInitializeAsync += OnInitializeAsync;
        }

        private new Task OnInitializeAsync(IProgress<ServiceProgressData> arg1, CancellationToken arg2)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
