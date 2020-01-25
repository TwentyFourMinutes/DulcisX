using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class InvalidVSStatusCodeException : Exception
    {
        public InvalidVSStatusCodeException() { }
        public InvalidVSStatusCodeException(int statusCode) : base($"Invalid VSStausCode: '{statusCode}'.") { }
        public InvalidVSStatusCodeException(string message) : base(message) { }
        public InvalidVSStatusCodeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidVSStatusCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
