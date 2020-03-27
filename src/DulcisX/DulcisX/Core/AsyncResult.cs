namespace DulcisX.Core
{
    /// <summary>
    /// Contains the result of an asynchronous operation.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AsyncResult<TResult>
    {
        private readonly TResult _result;
        private readonly bool _hasResult;

        internal AsyncResult(TResult result, bool hasResult)
        {
            _result = result;
            _hasResult = hasResult;
        }

        /// <summary>
        /// Gets the result of the asynchronous operation. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="result">The result of the operation.</param>
        /// <returns><see langword="true"/> if the operation returned a result; otherwise <see langword="false"/>.</returns>
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
