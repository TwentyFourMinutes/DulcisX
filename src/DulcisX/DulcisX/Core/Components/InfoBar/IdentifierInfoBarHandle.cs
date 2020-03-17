using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DulcisX.Core.Components
{
    public class ResultInfoBarHandle<TIdentifier> : InfoBarHandle
    {
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

        public async Task<AsyncResult<TIdentifier>> WaitForResultAsync(CancellationToken ct = default)
        {
            _semaphore = new SemaphoreSlim(0, 1);

            await _semaphore.WaitAsync(ct).ConfigureAwait(false);

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
