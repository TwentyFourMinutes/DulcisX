using DulcisX.Core.Models.Enums.VisualStudio;
using System;
using System.Threading;

namespace DulcisX.Nodes.Events
{
    public static class CancelTranslaterFactory
    {
        public static int Create<TCallback>(TCallback callback, ref int cancel, Action<CancelTraslaterToken> action, int statusCode = CommonStatusCodes.Success) where TCallback : class
        {
            if (callback is object)
            {
                var token = new CancelTraslaterToken();

                action.Invoke(token);

                cancel = token.CancelRequestedValue;
            }

            return statusCode;
        }
    }

    public class CancelTraslaterToken
    {
        private int _cancelRequested = 0;

        internal int CancelRequestedValue
        {
            get => Interlocked.CompareExchange(ref _cancelRequested, 1, 1);
            private set
            {
                if (value == 1)
                    Interlocked.CompareExchange(ref _cancelRequested, 1, 0);
                else
                    Interlocked.CompareExchange(ref _cancelRequested, 0, 1);
            }
        }

        public bool CancelRequested
        {
            get => CancelRequestedValue == 1;
        }

        public void Cancel()
        {
            CancelRequestedValue = 1;
        }
    }
}
