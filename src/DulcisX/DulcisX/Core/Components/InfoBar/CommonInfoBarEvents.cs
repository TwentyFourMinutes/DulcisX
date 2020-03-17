using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Components
{
    internal class InfoBarEvents : BaseInfoBarEvents
    {
        internal InfoBarEvents(InfoBar infoBar, IVsInfoBarUIElement uiElement) : base(infoBar, uiElement)
        {

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
