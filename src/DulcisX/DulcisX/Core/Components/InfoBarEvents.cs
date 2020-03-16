using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Components
{
    internal class InfoBarEvents : EventSink, IVsInfoBarUIEvents
    {
        private readonly InfoBar _infoBar;
        private readonly IVsInfoBarUIElement _uiElement;
        private bool _isDisposed;

        private InfoBarEvents(InfoBar infoBar, IVsInfoBarUIElement uiElement)
        {
            _infoBar = infoBar;
            _uiElement = uiElement;
        }

        public void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (actionItem.ActionContext is ActionCallback actionCallback)
            {
                actionCallback.Callback.Invoke();

                if (actionCallback.CloseAfterClick)
                {
                    _infoBar.RemoveMessage(new InfoBarHandle(infoBarUIElement, this));
                }
            }
            else if (actionItem.ActionContext is HyperLink hyperLink)
            {
                if (hyperLink.OpenInternally)
                {
                    _infoBar.WebBrowser.OpenInternal(hyperLink.Uri);
                }
                else
                {
                    _infoBar.WebBrowser.OpenExternal(hyperLink.Uri);
                }
            }
        }

        public void OnClosed(IVsInfoBarUIElement infoBarUIElement)
        {
            this.Dispose();
        }

        internal static InfoBarEvents Create(InfoBar infoBar, IVsInfoBarUIElement uiElement)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var infoBarEvents = new InfoBarEvents(infoBar, uiElement);

            var result = uiElement.Advise(infoBarEvents, out var cookie);

            ErrorHandler.ThrowOnFailure(result);

            infoBarEvents.SetCookie(cookie);

            return infoBarEvents;
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
