using System;

namespace DulcisX.Core
{
    internal class ActionCallback
    {
        internal Action Callback { get; }

        internal bool CloseAfterClick { get; }

        internal ActionCallback(Action callback, bool closeAfterClick)
        {
            Callback = callback;
            CloseAfterClick = closeAfterClick;
        }
    }
}
