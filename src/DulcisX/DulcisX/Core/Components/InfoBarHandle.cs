using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Components
{
    public class InfoBarHandle
    {
        internal IVsInfoBarUIElement UIElement { get; }
        internal InfoBarEvents Events { get; }

        internal InfoBarHandle(IVsInfoBarUIElement uiElement, InfoBarEvents events)
        {
            UIElement = uiElement;
            Events = events;
        }
    }
}
