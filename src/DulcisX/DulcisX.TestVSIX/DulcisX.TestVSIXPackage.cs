using System;
using System.Runtime.InteropServices;
using System.Threading;
using DulcisX.Components;
using System.Linq;
using DulcisX.Core;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using System.Collections.Generic;

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

        private void DocumentEvents_OnSave(DocumentX obj)
        {
            var test3 = Solution.Projects2;

            var test1 = ((IEnumerable<ProjectX>)Solution).ToList();

            var test4 = Solution.Projects.ToList();

            var test2 = obj.ParentProject;

            //var test1 = Solution.Projects.FirstOrDefault(x => x.FullName == "bcd");

            //var files = test1.ToList().First(x => x.FullName == "yeet");

            //var file = files.ToList();
        }
        #endregion
    }
}
