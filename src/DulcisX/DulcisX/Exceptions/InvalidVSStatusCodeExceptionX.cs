using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class InvalidVSStatusCodeExceptionX : Exception
    {
        public InvalidVSStatusCodeExceptionX() { }
        public InvalidVSStatusCodeExceptionX(int statusCode) : base($"Invalid VSStausCode: '{statusCode}'.") { }
        public InvalidVSStatusCodeExceptionX(string message) : base(message) { }
        public InvalidVSStatusCodeExceptionX(string message, Exception inner) : base(message, inner) { }
        protected InvalidVSStatusCodeExceptionX(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
