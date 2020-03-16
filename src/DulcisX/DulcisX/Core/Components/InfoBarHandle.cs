using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core.Components
{
    public class InfoBarHandle
    {
        internal IVsUIElement UIElement { get; }
        internal InfoBarEvents Events { get; }

        internal InfoBarHandle(IVsUIElement uiElement, InfoBarEvents events)
        {
            UIElement = uiElement;
            Events = events;
        }
    }
}
