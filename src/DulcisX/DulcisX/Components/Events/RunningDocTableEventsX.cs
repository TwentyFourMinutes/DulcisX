using DulcisX.Core.Models;
using DulcisX.Helpers;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    internal class RunningDocTableEventsX : BaseEventX, IRunningDocTableEventsX, IVsRunningDocTableEvents3
    {
        public event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
        public event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
        public event Action<HierarchyItemX> OnSaved;
        public event Action<HierarchyItemX, bool, IVsWindowFrame> OnDocumentWindowShow;
        public event Action<HierarchyItemX, IVsWindowFrame> OnDocumentWindowHidden;
        public event Action<HierarchyItemX, __VSRDTATTRIB, DocumentStateX, DocumentStateX> OnAttributeChanged;
        public event Action<HierarchyItemX> OnSave;

        private readonly IVsRunningDocumentTable _rdt;

        private RunningDocTableEventsX(IVsRunningDocumentTable rdt, SolutionX solution) : base(solution)
            => _rdt = rdt;

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            OnDocumentLocked?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution), (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            OnDocumentUnlocked?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution), (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            OnSaved?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution));
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            => VSConstants.S_OK;

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            OnDocumentWindowShow?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution), fFirstShow != 0, pFrame);
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            OnDocumentWindowHidden?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution), pFrame);
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            OnAttributeChanged?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution), (__VSRDTATTRIB)grfAttribs,
            new DocumentStateX
            {
                Hierarchy = pHierOld,
                Identifier = itemidOld,
                Name = pszMkDocumentOld
            },
            new DocumentStateX
            {
                Hierarchy = pHierNew,
                Identifier = itemidNew,
                Name = pszMkDocumentNew
            });
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            OnSave?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution));
            return VSConstants.S_OK;
        }

        internal void Destroy()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _rdt.UnadviseRunningDocTableEvents(CookieUID);
            VsHelper.ValidateSuccessStatusCode(result);
        }

        internal static IRunningDocTableEventsX Create(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = solution.ServiceProviders.GetService<SVsRunningDocumentTable, IVsRunningDocumentTable>();

            var rdtEvents = new RunningDocTableEventsX(rdt, solution);

            var result = rdt.AdviseRunningDocTableEvents(rdtEvents, out var cookieUID);

            VsHelper.ValidateSuccessStatusCode(result);

            rdtEvents.CookieUID = cookieUID;

            return rdtEvents;
        }
    }
}
