namespace DulcisX.Helpers
{
    /// <summary>
    /// Converts native method value results to their appropriate .Net Type and vice versa.
    /// </summary>
    public static class VsConverter
    {
        /// <summary>
        /// Converts an <see cref="int"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The value which should be converted.</param>
        /// <returns>A <see cref="bool"/> representation of the given <see cref="int"/>.</returns>
        public static bool AsBoolean(int value)
            => value == 1;

        /// <summary>
        /// Converts a <see cref="bool"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="value">The value which should be converted.</param>
        /// <returns>An <see cref="int"/> representation of the given <see cref="bool"/>.</returns>
        public static int FromBoolean(bool value)
            => value ? 1 : 0;
    }
}
