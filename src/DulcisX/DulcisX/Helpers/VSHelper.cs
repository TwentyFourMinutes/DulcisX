using DulcisX.Exceptions;
using Microsoft.VisualStudio;

namespace DulcisX.Helpers
{
    public static class VsHelper
    {
        public static void ValidateVSStatusCode(int statusCode)
        {
            if (statusCode != VSConstants.S_OK)
            {
                throw new InvalidVSStatusCodeException(statusCode);
            }
        }
    }
}
