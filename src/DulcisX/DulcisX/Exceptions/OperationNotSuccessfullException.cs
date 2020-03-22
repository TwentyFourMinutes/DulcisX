using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class OperationNotSuccessfulException : Exception
    {
        public OperationNotSuccessfulException() { }
        public OperationNotSuccessfulException(string message) : base(message) { }
        public OperationNotSuccessfulException(string message, Exception inner) : base(message, inner) { }
        protected OperationNotSuccessfulException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
