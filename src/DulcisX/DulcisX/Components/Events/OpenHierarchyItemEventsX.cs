using DulcisX.Helpers;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using DulcisX.Core.Models.Enums.VisualStudio;
using System.IO;

namespace DulcisX.Components.Events
{
    internal class OpenHierarchyItemEventsX : BaseEventX, IOpenHierarchyItemEventsX, IVsRunningDocTableEvents3
    {
        public event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentLocked;
        public event Action<HierarchyItemX, _VSRDTFLAGS, uint, uint> OnDocumentUnlocked;
        public event Action<HierarchyItemX> OnSaved;
        public event Action<HierarchyItemX, bool, IVsWindowFrame> OnDocumentWindowShow;
        public event Action<HierarchyItemX, IVsWindowFrame> OnDocumentWindowHidden;
        public event Action<HierarchyItemX, VsRDTAttributeX> OnAttributeChanged;
        public event Action<HierarchyItemX, string, string> OnRenamed;
        public event Action<HierarchyItemX, string, string> OnMoved;
        public event Action<HierarchyItemX> OnSave;

        private readonly IVsRunningDocumentTable _rdt;

        private OpenHierarchyItemEventsX(IVsRunningDocumentTable rdt, SolutionX solution) : base(solution)
            => _rdt = rdt;

        #region Internal Event Callbacks

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
            var hierarchyItem = _rdt.GetHierarchyItem(docCookie, Solution);

            var chgAttribute = (VsRDTAttributeX)grfAttribs;

            switch (chgAttribute)
            {
                case VsRDTAttributeX.MkDocument:
                    OnItemChangedFullName(hierarchyItem, pszMkDocumentOld, pszMkDocumentNew);
                    break;
            }

            OnAttributeChanged?.Invoke(hierarchyItem, chgAttribute);
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            OnSave?.Invoke(_rdt.GetHierarchyItem(docCookie, Solution));
            return VSConstants.S_OK;
        }

        #endregion

        #region Event Helper Methods

        private void OnItemChangedFullName(HierarchyItemX hierarchyItem, string oldName, string newName)
        {
            var oldFileName = Path.GetFileName(oldName);
            var newFileName = Path.GetFileName(newName);

            if (oldFileName != newFileName)
            {
                OnRenamed?.Invoke(hierarchyItem, oldFileName, newFileName);
                return;
            }

            var oldFilePath = Path.GetDirectoryName(oldName);
            var newFilePath = Path.GetDirectoryName(newName);


            if (oldFilePath != newFilePath)
            {
                OnMoved?.Invoke(hierarchyItem, oldFilePath, newFilePath);
            }
        }

        #endregion

        internal void Destroy()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _rdt.UnadviseRunningDocTableEvents(CookieUID);
            VsHelper.ValidateSuccessStatusCode(result);
        }

        internal static IOpenHierarchyItemEventsX Create(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var rdt = solution.ServiceProviders.GetService<SVsRunningDocumentTable, IVsRunningDocumentTable>();

            var rdtEvents = new OpenHierarchyItemEventsX(rdt, solution);

            var result = rdt.AdviseRunningDocTableEvents(rdtEvents, out var cookieUID);

            VsHelper.ValidateSuccessStatusCode(result);

            rdtEvents.CookieUID = cookieUID;

            return rdtEvents;
        }
    }
}
