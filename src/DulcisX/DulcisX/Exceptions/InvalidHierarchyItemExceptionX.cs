using System;

namespace DulcisX.Exceptions
{

    [Serializable]
    public class InvalidHierarchyItemExceptionX : Exception
    {
        public InvalidHierarchyItemExceptionX() { }
        public InvalidHierarchyItemExceptionX(string message) : base(message) { }
        public InvalidHierarchyItemExceptionX(string message, Exception inner) : base(message, inner) { }
        protected InvalidHierarchyItemExceptionX(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
