using DulcisX.Core;
using DulcisX.Core.Enums;
using DulcisX.Nodes;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
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
            this.Solution.OpenNodeEvents.OnSaved.Hook(NodeTypes.Document, OnDocumentSaved);
        }

        private void OnDocumentSaved(IPhysicalNode node)
        {
            this.InfoBar.NewMessage()
                        .WithInfoImage()
                        .WithText("The ")
                        .WithText(node.GetFullName(), underline: true)
                        .WithText(" file got saved.")
                        .WithButton("Okay")
                        .Publish();
        }
    }
}