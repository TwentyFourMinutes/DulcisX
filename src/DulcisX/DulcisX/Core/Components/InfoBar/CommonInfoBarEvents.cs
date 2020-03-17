using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Core.Components
{
    internal class InfoBarEvents : BaseInfoBarEvents
    {
        private readonly Action _cancelCallback;

        internal InfoBarEvents(InfoBar infoBar, IVsInfoBarUIElement uiElement, Action cancelCallback) : base(infoBar, uiElement)
        {
            _cancelCallback = cancelCallback;

            if (_cancelCallback is object)
            {
                base.OnMessageClosed += OnMessageClosed;
            }
        }

        private new void OnMessageClosed()
        {
            _cancelCallback.Invoke();

            base.OnMessageClosed -= OnMessageClosed;
        }

        public override void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            if (actionItem.ActionContext is ActionCallback actionCallback)
            {
                actionCallback.Callback?.Invoke();

                if (actionCallback.CloseAfterClick)
                {
                    InfoBar.RemoveMessage(infoBarUIElement, this);
                }
            }
            else if (actionItem.ActionContext is HyperLink hyperLink)
            {
                if (hyperLink.OpenInternally)
                {
                    InfoBar.WebBrowser.OpenInternal(hyperLink.Uri);
                }
                else
                {
                    InfoBar.WebBrowser.OpenExternal(hyperLink.Uri);
                }
            }
        }
    }
}
