using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DulcisX.Core.Components
{
    /// <summary>
    /// Represents a result Handler of an InforBar message.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the button identifier.</typeparam>
    public class ResultInfoBarHandle<TIdentifier> : InfoBarHandle
    {
        /// <summary>
        /// Occurs when the user produces any kind of result.
        /// </summary>
        public event Action<TIdentifier, bool> OnResult;

        private readonly ResultInfoBarEvents<TIdentifier> _events;
        private SemaphoreSlim _semaphore;
        private TIdentifier _identifier;
        private bool _isIdentifierSet;
        private bool _isDisposed;

        internal ResultInfoBarHandle(IVsInfoBarUIElement uiElement, ResultInfoBarEvents<TIdentifier> events) : base(uiElement, events)
        {
            _events = events;
            _events.OnInfoBarResult += OnInfoBarResult;
            _events.OnMessageClosed += OnMessageClosed;
        }

        private void OnMessageClosed()
        {
            OnResult?.Invoke(_identifier, !_isIdentifierSet);
            InternalDispose();
        }

        private void OnInfoBarResult(TIdentifier identifier)
        {
            _identifier = identifier;
            _isIdentifierSet = true;

            if (_semaphore is object)
            {
                _semaphore.Release();
            }

            OnResult?.Invoke(_identifier, true);
        }

        /// <summary>
        /// Asynchronously waits for an result which is produced by the user.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>An <see cref="AsyncResult{TIdentifier}"/> containg the <typeparamref name="TIdentifier"/> of the clicked button.</returns>
        public async Task<AsyncResult<TIdentifier>> WaitForResultAsync(CancellationToken ct = default)
        {
            _semaphore = new SemaphoreSlim(0, 1);

            await _semaphore.WaitAsync(ct);

            this.InternalDispose();

            if (_isIdentifierSet)
            {
                return AsyncResult.FromResult(_identifier);
            }
            else
            {
                return AsyncResult.FromError<TIdentifier>();
            }
        }

        private void InternalDispose()
        {
            if (!_isDisposed)
            {
                if (_semaphore is object)
                {
                    _semaphore.Release();

                    _semaphore.Dispose();
                }

                _events.OnInfoBarResult -= OnInfoBarResult;
                _events.OnMessageClosed -= OnMessageClosed;

                _isDisposed = true;
            }
        }
    }
}
