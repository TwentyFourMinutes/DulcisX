using DulcisX.Core.Extensions;
using DulcisX.Core.Models.Enums.VisualStudio;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;

namespace DulcisX.Nodes.Events
{
    internal class OpenNodeEvents : EventSink, IOpenNodeEvents, IVsRunningDocTableEvents3
    {
        #region Events

        private EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>> _onNodeLocked;
        public EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>> OnNodeLocked
            => _onNodeLocked ?? (_onNodeLocked = new EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>>());

        private EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>> _onNodeUnlocked;
        public EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>> OnNodeUnlocked
            => _onNodeUnlocked ?? (_onNodeUnlocked = new EventDistributor<Action<BaseNode, _VSRDTFLAGS, uint, uint>>());

        private EventDistributor<Action<BaseNode>> _onSaved;
        public EventDistributor<Action<BaseNode>> OnSaved
            => _onSaved ?? (_onSaved = new EventDistributor<Action<BaseNode>>());

        private EventDistributor<Action<BaseNode, bool, IVsWindowFrame>> _onNodeWindowShow;
        public EventDistributor<Action<BaseNode, bool, IVsWindowFrame>> OnNodeWindowShow
            => _onNodeWindowShow ?? (_onNodeWindowShow = new EventDistributor<Action<BaseNode, bool, IVsWindowFrame>>());

        private EventDistributor<Action<BaseNode, IVsWindowFrame>> _onNodeWindowHidden;
        public EventDistributor<Action<BaseNode, IVsWindowFrame>> OnNodeWindowHidden
            => _onNodeWindowHidden ?? (_onNodeWindowHidden = new EventDistributor<Action<BaseNode, IVsWindowFrame>>());

        private EventDistributor<Action<BaseNode, VsRDTAttribute>> _onAttributeChanged;
        public EventDistributor<Action<BaseNode, VsRDTAttribute>> OnAttributeChanged
            => _onAttributeChanged ?? (_onAttributeChanged = new EventDistributor<Action<BaseNode, VsRDTAttribute>>());

        private EventDistributor<Action<BaseNode, string, string>> _onRenamed;
        public EventDistributor<Action<BaseNode, string, string>> OnRenamed =>
            _onRenamed ?? (_onRenamed = new EventDistributor<Action<BaseNode, string, string>>());

        private EventDistributor<Action<BaseNode, string, string>> _onMoved;
        public EventDistributor<Action<BaseNode, string, string>> OnMoved =>
            _onMoved ?? (_onMoved = new EventDistributor<Action<BaseNode, string, string>>());

        private EventDistributor<Action<BaseNode>> _onSave;
        public EventDistributor<Action<BaseNode>> OnSave
            => _onSave ?? (_onSave = new EventDistributor<Action<BaseNode>>());

        #endregion

        private readonly IVsRunningDocumentTable _documentTable;

        private OpenNodeEvents(SolutionNode solution, IVsRunningDocumentTable documentTable) : base(solution)
        {
            _documentTable = documentTable;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            if (_onNodeLocked is null)
                return CommonStatusCodes.Success;

            var node = _documentTable.GetNode(docCookie, Solution);

            _onNodeLocked.Invoke(node.NodeType, node, (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);

            return CommonStatusCodes.Success;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            if (_onNodeUnlocked is null)
                return CommonStatusCodes.Success;

            var node = _documentTable.GetNode(docCookie, Solution);

            _onNodeUnlocked.Invoke(node.NodeType, node, (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);

            return CommonStatusCodes.Success;
        }

        public int OnAfterSave(uint docCookie)
        {
            if (_onSaved is null)
                return CommonStatusCodes.Success;

            var node = _documentTable.GetNode(docCookie, Solution);

            _onSaved.Invoke(node.NodeType, node);

            return CommonStatusCodes.Success;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            => CommonStatusCodes.Success;

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            if (_onNodeWindowShow is null)
                return CommonStatusCodes.Success;

            var node = _documentTable.GetNode(docCookie, Solution);

            _onNodeWindowShow.Invoke(node.NodeType, node, fFirstShow != 0, pFrame);

            return CommonStatusCodes.Success;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            if (_onNodeWindowHidden is null)
                return CommonStatusCodes.Success;

            var node = _documentTable.GetNode(docCookie, Solution);

            _onNodeWindowHidden.Invoke(node.NodeType, node, pFrame);

            return CommonStatusCodes.Success;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            var node = new Lazy<BaseNode>(() => _documentTable.GetNode(docCookie, Solution));

            var attribute = (VsRDTAttribute)grfAttribs;

            _onAttributeChanged?.Invoke(node.Value.NodeType, node.Value, attribute);

            if (_onRenamed is null && _onMoved is null)
                return CommonStatusCodes.Success;

            switch (attribute)
            {
                case VsRDTAttribute.MkDocument:
                    OnItemChangedFullName(node, pszMkDocumentOld, pszMkDocumentNew);
                    break;
            }

            return CommonStatusCodes.Success;
        }

        private void OnItemChangedFullName(Lazy<BaseNode> node, string oldName, string newName)
        {
            var oldFileName = Path.GetFileName(oldName);
            var newFileName = Path.GetFileName(newName);

            if (oldFileName != newFileName && _onRenamed is object)
            {
                _onRenamed.Invoke(node.Value.NodeType, node.Value, oldFileName, newFileName);
                return;
            }

            var oldFilePath = Path.GetDirectoryName(oldName);
            var newFilePath = Path.GetDirectoryName(newName);


            if (oldFilePath != newFilePath && _onMoved is object)
            {
                _onMoved.Invoke(node.Value.NodeType, node.Value, oldFilePath, newFilePath);
            }
        }

        public int OnBeforeSave(uint docCookie)
        {
            if (_onNodeLocked is null)
                return CommonStatusCodes.Success;

            var node = _documentTable.GetNode(docCookie, Solution);

            _onSave.Invoke(node.NodeType, node);

            return CommonStatusCodes.Success;
        }

        internal static IOpenNodeEvents Create(SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var documentTable = solution.ServiceContainer.GetCOMInstance<IVsRunningDocumentTable>();

            var rdtEvents = new OpenNodeEvents(solution, documentTable);

            var result = documentTable.AdviseRunningDocTableEvents(rdtEvents, out var cookieUID);

            ErrorHandler.ThrowOnFailure(result);

            rdtEvents.SetCookie(cookieUID);

            return rdtEvents;
        }

        public override void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _documentTable.UnadviseRunningDocTableEvents(Cookie);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}
