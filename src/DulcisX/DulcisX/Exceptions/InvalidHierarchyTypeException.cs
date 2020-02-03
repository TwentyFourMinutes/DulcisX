using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class InvalidHierarchyTypeException : Exception
    {
        public InvalidHierarchyTypeException() { }
        public InvalidHierarchyTypeException(string message) : base(message) { }
        public InvalidHierarchyTypeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidHierarchyTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
