using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;

namespace DulcisX.Nodes.Events
{
    internal class OpenNodeEvents : NodeEventSink, IOpenNodeEvents, IVsRunningDocTableEvents3
    {
        #region Events

        private EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> _onNodeLocked;
        public EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> OnNodeLocked
            => _onNodeLocked ?? (_onNodeLocked = new EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>>());

        private EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> _onNodeUnlocked;
        public EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>> OnNodeUnlocked
            => _onNodeUnlocked ?? (_onNodeUnlocked = new EventDistributor<Action<IPhysicalNode, _VSRDTFLAGS, uint, uint>>());

        private EventDistributor<Action<IPhysicalNode>> _onSaved;
        public EventDistributor<Action<IPhysicalNode>> OnSaved
            => _onSaved ?? (_onSaved = new EventDistributor<Action<IPhysicalNode>>());

        private EventDistributor<Action<IPhysicalNode, bool, IVsWindowFrame>> _onNodeWindowShow;
        public EventDistributor<Action<IPhysicalNode, bool, IVsWindowFrame>> OnNodeWindowShow
            => _onNodeWindowShow ?? (_onNodeWindowShow = new EventDistributor<Action<IPhysicalNode, bool, IVsWindowFrame>>());

        private EventDistributor<Action<IPhysicalNode, IVsWindowFrame>> _onNodeWindowHidden;
        public EventDistributor<Action<IPhysicalNode, IVsWindowFrame>> OnNodeWindowHidden
            => _onNodeWindowHidden ?? (_onNodeWindowHidden = new EventDistributor<Action<IPhysicalNode, IVsWindowFrame>>());

        private EventDistributor<Action<IPhysicalNode, OpenNodeAttribute>> _onAttributeChanged;
        public EventDistributor<Action<IPhysicalNode, OpenNodeAttribute>> OnAttributeChanged
            => _onAttributeChanged ?? (_onAttributeChanged = new EventDistributor<Action<IPhysicalNode, OpenNodeAttribute>>());

        private EventDistributor<Action<IPhysicalNode, string, string>> _onRenamed;
        public EventDistributor<Action<IPhysicalNode, string, string>> OnRenamed =>
            _onRenamed ?? (_onRenamed = new EventDistributor<Action<IPhysicalNode, string, string>>());

        private EventDistributor<Action<IPhysicalNode, string, string>> _onMoved;
        public EventDistributor<Action<IPhysicalNode, string, string>> OnMoved =>
            _onMoved ?? (_onMoved = new EventDistributor<Action<IPhysicalNode, string, string>>());

        private EventDistributor<Action<IPhysicalNode>> _onSave;
        public EventDistributor<Action<IPhysicalNode>> OnSave
            => _onSave ?? (_onSave = new EventDistributor<Action<IPhysicalNode>>());

        #endregion

        private readonly IVsRunningDocumentTable _documentTable;

        private OpenNodeEvents(SolutionNode solution, IVsRunningDocumentTable documentTable) : base(solution)
        {
            _documentTable = documentTable;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            if (_onNodeLocked is object)
            {
                var node = _documentTable.GetNode(docCookie, Solution);

                _onNodeLocked.Invoke(node.NodeType, node, (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);
            }

            return CommonStatusCodes.Success;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            if (_onNodeUnlocked is object)
            {
                var node = _documentTable.GetNode(docCookie, Solution);

                _onNodeUnlocked.Invoke(node.NodeType, node, (_VSRDTFLAGS)dwRDTLockType, dwReadLocksRemaining, dwEditLocksRemaining);
            }

            return CommonStatusCodes.Success;
        }

        public int OnAfterSave(uint docCookie)
        {
            if (_onSaved is object)
            {
                var node = _documentTable.GetNode(docCookie, Solution);

                _onSaved.Invoke(node.NodeType, node);
            }

            return CommonStatusCodes.Success;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            => CommonStatusCodes.NotImplemented;

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            if (_onNodeWindowShow is object)
            {
                var node = _documentTable.GetNode(docCookie, Solution);

                _onNodeWindowShow.Invoke(node.NodeType, node, fFirstShow != 0, pFrame);
            }

            return CommonStatusCodes.Success;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            if (_onNodeWindowHidden is object)
            {
                var node = _documentTable.GetNode(docCookie, Solution);

                _onNodeWindowHidden.Invoke(node.NodeType, node, pFrame);
            }

            return CommonStatusCodes.Success;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            var node = new Lazy<IPhysicalNode>(() => _documentTable.GetNode(docCookie, Solution));

            var attribute = (OpenNodeAttribute)grfAttribs;

            _onAttributeChanged?.Invoke(node.Value.NodeType, node.Value, attribute);

            if (_onRenamed is null && _onMoved is null)
                return CommonStatusCodes.Success;

            switch (attribute)
            {
                case OpenNodeAttribute.MkDocument:
                    OnItemChangedFullName(node, pszMkDocumentOld, pszMkDocumentNew);
                    break;
            }

            return CommonStatusCodes.Success;
        }

        private void OnItemChangedFullName(Lazy<IPhysicalNode> node, string oldName, string newName)
        {
            var oldFileName = Path.GetFileName(oldName);
            var newFileName = Path.GetFileName(newName);

            if (_onRenamed is object &&
                oldFileName != newFileName)
            {
                _onRenamed.Invoke(node.Value.NodeType, node.Value, oldFileName, newFileName);
                return;
            }

            var oldFilePath = Path.GetDirectoryName(oldName);
            var newFilePath = Path.GetDirectoryName(newName);

            if (_onMoved is object &&
                oldFilePath != newFilePath)
            {
                _onMoved.Invoke(node.Value.NodeType, node.Value, oldFilePath, newFilePath);
            }
        }

        public int OnBeforeSave(uint docCookie)
        {
            if (_onSave is object)
            {
                var node = _documentTable.GetNode(docCookie, Solution);

                _onSave.Invoke(node.NodeType, node);
            }

            return CommonStatusCodes.Success;
        }

        internal static IOpenNodeEvents Create(SolutionNode solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var documentTable = solution.RunningDocumentTable;

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
