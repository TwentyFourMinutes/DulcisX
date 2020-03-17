using System;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Core;
using DulcisX.Core.Enums;
using DulcisX.Nodes;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

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
            OnInitializeAsync += DulcisXTestVSIXPackage_OnInitializeAsync;
        }

        private async System.Threading.Tasks.Task DulcisXTestVSIXPackage_OnInitializeAsync(CancellationToken arg, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(arg);

            Solution.OpenNodeEvents.OnSave.Hook(NodeTypes.Document, Saved);
        }

        private async void Saved(IPhysicalNode savedNode)
        {
            var result = InfoBar.NewMessage()
                                .WithInfoImage()
                                .WithText("Is this code a good boi?", true, false, true)
                                .WithHyperlink(" plz food.", () =>
                                {

                                })
                                .WithHyperlink(" plz food link.", new Uri("https://www.twenty-four.dev"), true)
                                .WithButton("Yes")
                                .WithButton("Yes, but actually no")
                                .Publish(() =>
                                {

                                });
        }

        #endregion
    }
}