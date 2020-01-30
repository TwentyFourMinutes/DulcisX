using DulcisX.Core.Models;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    internal class RunningDocTableEventsX : EventCookieX, IRunningDocTableEventsX, IVsRunningDocTableEvents3
    {
        public event Action<uint, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
        public event Action<uint, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
        public event Action<uint> OnSaved;
        public event Action<uint, bool, IVsWindowFrame> OnDocumentWindowShow;
        public event Action<uint, IVsWindowFrame> OnDocumentWindowHidden;
        public event Action<uint, __VSRDTATTRIB, DocumentStateX, DocumentStateX> OnAttributeChanged;
        public event Action<uint> OnSave;

        private readonly IVsRunningDocumentTable _rdt;

        private RunningDocTableEventsX(IVsRunningDocumentTable rdt)
            => _rdt = rdt;

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            OnDocumentLocked?.Invoke(docCookie, (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            OnDocumentUnlocked?.Invoke(docCookie, (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            OnSaved?.Invoke(docCookie);
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            => VSConstants.S_OK;

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            OnDocumentWindowShow?.Invoke(docCookie, fFirstShow != 0, pFrame);
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            OnDocumentWindowHidden?.Invoke(docCookie, pFrame);
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            OnAttributeChanged?.Invoke(docCookie, (__VSRDTATTRIB)grfAttribs,
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
            OnSave?.Invoke(docCookie);
            return VSConstants.S_OK;
        }

        internal void Destroy()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _rdt.UnadviseRunningDocTableEvents(CookieUID);
            VsHelper.ValidateVSStatusCode(result);
        }

        internal static IRunningDocTableEventsX Create(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = solution.ServiceProviders.GetService<SVsRunningDocumentTable, IVsRunningDocumentTable>();

            var rdtEvents = new RunningDocTableEventsX(rdt);

            var result = rdt.AdviseRunningDocTableEvents(rdtEvents, out var cookieUID);

            VsHelper.ValidateVSStatusCode(result);

            rdtEvents.CookieUID = cookieUID;

            return rdtEvents;
        }
    }
}
