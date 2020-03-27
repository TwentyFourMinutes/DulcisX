namespace DulcisX.Core
{
    /// <summary>
    /// Provides a simple container for a Com Type.
    /// </summary>
    public class ComContainer<TComType>
    {
        /// <summary>
        /// Gets the wrapped Com instance.
        /// </summary>
        public TComType Value { get; }

        internal ComContainer(TComType comType)
        {
            Value = comType;
        }
    }

    /// <summary>
    /// Provides a simple container for a Com Type.
    /// </summary>
    public static class ComContainer
    {
        /// <summary>
        /// Creates a new <see cref="ComContainer{TComType}"/> instance.
        /// </summary>
        /// <typeparam name="TComType">The type of the Com Type.</typeparam>
        /// <param name="comType">The instance which should wrapped.</param>
        /// <returns>A new <see cref="ComContainer{TComType}"/> instance with the provided <paramref name="comType"/>.</returns>
        public static ComContainer<TComType> Create<TComType>(TComType comType)
        {
            return new ComContainer<TComType>(comType);
        }
    }
}
