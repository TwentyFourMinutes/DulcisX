using System;
using System.Runtime.InteropServices;
using DulcisX.Core;
using DulcisX.Core.Extensions;
using DulcisX.Nodes;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

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

        private async System.Threading.Tasks.Task DulcisXTestVSIXPackage_OnInitializeAsync(System.Threading.CancellationToken arg)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(arg);

            GetSolution().UnderlyingHierarchy.AdviseHierarchyEvents(new Poggers(), out _);

            foreach (var test in GetSolution().GetAllProjects())
            {
                test.UnderlyingHierarchy.AdviseHierarchyEvents(new Poggers(), out _);
            }

            GetSolution().UnderlyingSolution.AdviseSolutionEvents(new Yaeeet(GetSolution()), out _);
        }

        #endregion
    }

    public class Yaeeet : IVsSolutionEvents, IVsSolutionEvents5
    {
        SolutionNode solution;
        public Yaeeet(SolutionNode so)
        {
            solution = so;
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            if (fAdded == 0)
                return 0;

            var kekW = solution.GetProject(pHierarchy);
            if (_lastStringLoad is object &&
                kekW.GetFullName() == _lastStringLoad)
            {

            }

            return 0;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return 0;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            if (fRemoved == 0)
                return 0;

            var pro = solution.GetProject(pHierarchy);
            var kek = pro.IsLoaded();
            var kek2 = pro.GetDisplayName();
            return 0;
        }

        private string _lastStringLoad;

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {

            return 0;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return 0;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {

            return 0;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return 0;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return 0;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return 0;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return 0;
        }

        //Project Added
        public void OnBeforeOpenProject(ref Guid guidProjectID, ref Guid guidProjectType, string pszFileName)
        {
            if (guidProjectType == VSConstants.CLSID.MiscellaneousFilesProject_guid ||
                guidProjectID != Guid.Empty)
            {
                return;
            }

            _lastStringLoad = pszFileName;
        }
    }

    public class Poggers : IVsHierarchyEvents, IVsHierarchyEvents2
    {
        public int OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded)
        {
            return 0;
        }

        public int OnItemsAppended(uint itemidParent)
        {
            return 0;
        }

        public int OnItemDeleted(uint itemid)
        {
            return 0;
        }

        public int OnPropertyChanged(uint itemid, int propid, uint flags)
        {
            return 0;
        }

        public int OnInvalidateItems(uint itemidParent)
        {
            return 0;
        }

        public int OnInvalidateIcon(IntPtr hicon)
        {
            return 0;
        }

        public int OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded, bool ensureVisible)
        {
            return 0;
        }
    }
}