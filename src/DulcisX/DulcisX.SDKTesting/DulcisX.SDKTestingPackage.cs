using DulcisX.Core;
using DulcisX.Core.Enums;
using DulcisX.Nodes;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DulcisX.SDKTesting
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class MyFirstExtensionPackage : PackageX
    {
        public const string PackageGuidString = "8a3d2f48-ee2b-4b8e-aa2b-430e3e2fc24e";

        public MyFirstExtensionPackage()
        {
            this.OnInitializeAsync += MyFirstExtensionPackage_OnInitializeAsync;
        }

        private async System.Threading.Tasks.Task MyFirstExtensionPackage_OnInitializeAsync(System.Threading.CancellationToken arg1, System.IProgress<ServiceProgressData> arg2)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            this.Solution.OpenNodeEvents.OnSaved.Hook(NodeTypes.Document, OnDocumentSaved);
        }

        private async void OnDocumentSaved(IPhysicalNode node)
        {
            var n = ((DocumentNode)node);


        }

    }
}