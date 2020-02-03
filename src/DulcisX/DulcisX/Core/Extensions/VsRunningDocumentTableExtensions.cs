using DulcisX.Components;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Core.Extensions
{
    public static class VsRunningDocumentTableExtensions
    {
        public static DocumentX GetDocument(this IVsRunningDocumentTable rdt, uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = rdt.GetDocumentInfo(docCookie, out _, out _, out _, out _, out var hierarchy, out uint itemId, out _);
            VsHelper.ValidateSuccessStatusCode(result);

            return new DocumentX(hierarchy, itemId);
        }
    }
}
