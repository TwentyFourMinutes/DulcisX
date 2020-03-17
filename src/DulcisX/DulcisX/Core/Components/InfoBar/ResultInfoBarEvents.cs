using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Core.Components
{
    internal class ResultInfoBarEvents<TIdentifier> : BaseInfoBarEvents
    {
        internal event Action<TIdentifier> OnInfoBarResult;

        internal ResultInfoBarEvents(InfoBar infoBar, IVsInfoBarUIElement uiElement) : base(infoBar, uiElement)
        {
        }

        public override void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            if (actionItem.ActionContext is TIdentifier identifier)
            {
                OnInfoBarResult?.Invoke(identifier);

                InfoBar.RemoveMessage(infoBarUIElement, this);
            }
        }
    }
}
