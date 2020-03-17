namespace DulcisX.Core
{
    public class AsyncResult<TResult>
    {
        private readonly TResult _result;
        private readonly bool _hasResult;

        internal AsyncResult(TResult result, bool hasResult)
        {
            _result = result;
            _hasResult = hasResult;
        }

        public bool TryGetResult(out TResult result)
        {
            result = _result;

            return _hasResult;
        }
    }

    internal static class AsyncResult
    {
        internal static AsyncResult<TResult> FromResult<TResult>(TResult result)
            => new AsyncResult<TResult>(result, true);

        internal static AsyncResult<TResult> FromError<TResult>()
            => new AsyncResult<TResult>(default, false);
    }
}
