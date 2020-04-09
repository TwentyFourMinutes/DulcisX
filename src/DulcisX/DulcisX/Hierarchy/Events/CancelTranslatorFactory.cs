using DulcisX.Core.Enums;
using System;
using System.Threading;

namespace DulcisX.Hierarchy.Events
{
    internal static class CancelTranslaterFactory
    {
        internal static int Create<TCallback>(TCallback callback, ref int cancel, Action<CancelTraslaterToken> action, int statusCode = CommonStatusCodes.Success) where TCallback : class
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

    /// <summary>
    /// Allows for cancellation of an operation.
    /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the operation was requested to cancel or not.
        /// </summary>
        public bool CancelRequested
        {
            get => CancelRequestedValue == 1;
        }

        /// <summary>
        /// Reuqests to cancel the current operation.
        /// </summary>
        public void Cancel()
        {
            CancelRequestedValue = 1;
        }
    }
}
