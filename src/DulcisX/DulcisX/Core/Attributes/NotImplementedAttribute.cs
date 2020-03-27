using System;

namespace DulcisX.Core.Attributes
{
    /// <summary>
    /// Marks a class or a member as not yet fully implemented.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public sealed class NotImplementedAttribute : Attribute
    {
        /// <summary>
        /// Gets the reason for the current <see cref="NotImplementedException"/> instance.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedAttribute"/> class with a specified reason.
        /// </summary>
        /// <param name="reason">The text string that describes the reason.</param>
        public NotImplementedAttribute(string reason)
        {
            Reason = reason;
        }
    }
}
