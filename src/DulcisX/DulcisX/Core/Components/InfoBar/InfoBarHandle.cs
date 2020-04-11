using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core
{
    /// <summary>
    /// Points to an InfoBar message and serves as a unique pointer to this message.
    /// </summary>
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
