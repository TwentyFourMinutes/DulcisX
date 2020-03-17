using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Components
{
    public class InfoBarHandle
    {
        internal IVsInfoBarUIElement UIElement { get; }

        internal BaseInfoBarEvents Events { get; }

        internal InfoBarHandle(IVsInfoBarUIElement uiElement, BaseInfoBarEvents events)
        {
            UIElement = uiElement;
            Events = events;
        }
    }
}
