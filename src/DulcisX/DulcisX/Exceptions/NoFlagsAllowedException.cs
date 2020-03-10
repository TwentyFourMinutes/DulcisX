using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class NoFlagsAllowedException : Exception
    {
        public NoFlagsAllowedException() { }
        public NoFlagsAllowedException(string enumType) : base($"No flags for {enumType} are allowed.") { }

        public NoFlagsAllowedException(string message, Exception inner) : base(message, inner) { }
        protected NoFlagsAllowedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
