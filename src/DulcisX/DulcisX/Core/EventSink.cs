using System;

namespace DulcisX.Core
{
    public abstract class EventSink : IDisposable
    {
        public uint Cookie { get; private set; }

        private bool _isCookieSet;

        protected void SetCookie(uint cookie)
        {
            if (!_isCookieSet)
            {
                Cookie = cookie;
                _isCookieSet = true;
            }
            else
            {
                throw new InvalidOperationException("Cookie is already set. You can only call this method once for an object.");
            }
        }

        public abstract void Dispose();
    }
}
