using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Core;
using DulcisX.Core.Models.Enums;
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

            GetSolution().OpenNodeEvents.OnSave.Hook(NodeTypes.Document, Saved);
        }

        private void Saved(IPhysicalNode savedNode)
        {
            var node = (DocumentNode)savedNode;

            var parentProject = node.GetParentProject();

            var displayName = parentProject.GetDisplayName();

            var projects = GetSolution().GetAllProjects();

            var children = GetSolution().GetChildren();
        }

        #endregion
    }
}