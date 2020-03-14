using System;

namespace DulcisX.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public sealed class NotImplementedAttribute : Attribute
    {
        public string Reason { get; }

        public NotImplementedAttribute(string reason)
        {
            Reason = reason;
        }
    }
}
