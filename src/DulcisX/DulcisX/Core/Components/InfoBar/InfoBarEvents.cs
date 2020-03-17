using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Core.Components
{
    internal abstract class BaseInfoBarEvents : EventSink, IVsInfoBarUIEvents
    {
        internal event Action OnMessageClosed;
        protected InfoBar InfoBar { get; }
        private readonly IVsInfoBarUIElement _uiElement;
        private bool _isDisposed;

        protected BaseInfoBarEvents(InfoBar infoBar, IVsInfoBarUIElement uiElement)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            InfoBar = infoBar;
            _uiElement = uiElement;

            var result = uiElement.Advise(this, out var cookie);

            ErrorHandler.ThrowOnFailure(result);

            this.SetCookie(cookie);
        }

        public abstract void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem);

        public void OnClosed(IVsInfoBarUIElement infoBarUIElement)
        {
            this.Dispose();
            OnMessageClosed?.Invoke();
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var result = _uiElement.Unadvise(Cookie);

                ErrorHandler.ThrowOnFailure(result);

                _isDisposed = true;
            }
        }
    }
}
