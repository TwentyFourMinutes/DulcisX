using Microsoft.VisualStudio;

namespace DulcisX.Core.Enums
{
    /// <summary>
    /// Common HResults of native methods.
    /// </summary>
    public static class CommonStatusCodes
    {
        /// <summary>
        /// Represents a HResult which indicates the generic success of a native method.
        /// </summary>
        public const int Success = VSConstants.S_OK;
        /// <summary>
        /// Represents a HResult which indicates the generic failure of a native method.
        /// </summary>
        public const int Failure = VSConstants.E_FAIL;
    }
}
