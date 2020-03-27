using System;

namespace DulcisX.Core
{
    /// <summary>
    /// Provides basic logic for the inheritance of native Visual Studio Events.
    /// </summary>
    public abstract class EventSink : IDisposable
    {
        /// <summary>
        /// Gets the Cookie which identifies the current <see cref="EventSink"/>.
        /// </summary>
        public uint Cookie { get; private set; }

        private bool _isCookieSet;

        /// <summary>
        /// Sets the Cookie of the <see cref="EventSink"/>.
        /// </summary>
        /// <param name="cookie">The cookie which is used to identify the <see cref="EventSink"/>.</param>
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

        /// <summary>
        /// Disposes the current <see cref="EventSink"/>.
        /// </summary>
        public abstract void Dispose();
    }
}
