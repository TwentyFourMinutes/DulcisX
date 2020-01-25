using DulcisX.Components;
using DulcisX.Exceptions;
using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;

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
