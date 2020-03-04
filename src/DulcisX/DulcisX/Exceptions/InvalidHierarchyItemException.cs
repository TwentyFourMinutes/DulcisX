using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class InvalidHierarchyItemException : Exception
    {
        public InvalidHierarchyItemException() { }
        public InvalidHierarchyItemException(string message) : base(message) { }
        public InvalidHierarchyItemException(string message, Exception inner) : base(message, inner) { }
        protected InvalidHierarchyItemException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
