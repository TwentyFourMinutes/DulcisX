using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class InvalidHierarchyTypeExceptionX : Exception
    {
        public InvalidHierarchyTypeExceptionX() { }
        public InvalidHierarchyTypeExceptionX(string message) : base(message) { }
        public InvalidHierarchyTypeExceptionX(string message, Exception inner) : base(message, inner) { }
        protected InvalidHierarchyTypeExceptionX(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
