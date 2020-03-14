using System;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private async void Saved(IPhysicalNode savedNode)
        {
            var solution = GetSolution();

            var test = await solution.GetAllChildrenAsync();

        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public sealed class NotImplementedAttribute : Attribute
    {
        public string Reason { get; }

        public NotImplementedAttribute(string reason)
        {
            Reason = reason;
        }
    }
}