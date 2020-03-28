using System;

namespace DulcisX.Exceptions
{
    /// <summary>
    /// The exception that is thrown for calls to class members which encounter an invald HResult.
    /// </summary>
    [Serializable]
    public class OperationNotSuccessfulException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OperationNotSuccessfulException"/> class.
        /// </summary>
        public OperationNotSuccessfulException() { }

        /// <summary>
        /// Initializes a new instance of <see cref="OperationNotSuccessfulException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OperationNotSuccessfulException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="OperationNotSuccessfulException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public OperationNotSuccessfulException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of <see cref="OperationNotSuccessfulException"/> class.
        /// </summary>
        /// <param name="info"> The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException"><paramref name="info"/> is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or <see cref="Exception.HResult"/> is zero (0).</exception>
        protected OperationNotSuccessfulException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
