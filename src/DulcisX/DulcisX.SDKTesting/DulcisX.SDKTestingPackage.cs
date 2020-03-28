using System;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Core;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace DulcisX.SDKTesting
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class SDKTestingPackage : PackageX
    {
        public const string PackageGuidString = "8a3d2f48-ee2b-4b8e-aa2b-430e3e2fc24e";

        #region Package Members

        public SDKTestingPackage()
        {
            OnInitializeAsync += DulcisXTestVSIXPackage_OnInitializeAsync;
        }

        private async System.Threading.Tasks.Task DulcisXTestVSIXPackage_OnInitializeAsync(CancellationToken arg, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(arg);

            Solution.SolutionEvents.OnProjectOpened.Hook(Core.Enums.NodeTypes.All, (node, boolean) =>
            {

            });
        }
        #endregion
    }
}
