using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Core.Components
{
    public class WebBrowser
    {
        private readonly IVsWebBrowsingService _browsingService;

        internal WebBrowser(IVsWebBrowsingService browsingService)
        {
            _browsingService = browsingService;
        }

        public IVsWindowFrame OpenInternal(Uri uri, __VSWBNAVIGATEFLAGS flags = 0)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _browsingService.Navigate(uri.AbsoluteUri, (uint)flags, out var frame);

            ErrorHandler.ThrowOnFailure(result);

            return frame;
        }

        public void OpenExternal(Uri uri, __VSCREATEWEBBROWSER creationFlags = __VSCREATEWEBBROWSER.VSCWB_ForceNew, VSPREVIEWRESOLUTION previewFlags = VSPREVIEWRESOLUTION.PR_Default)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _browsingService.CreateExternalWebBrowser((uint)creationFlags, previewFlags, uri.AbsoluteUri);

            ErrorHandler.ThrowOnFailure(result);
        }

        public IEnumerable<IVsWindowFrame> OpenInternalBrowserWindows()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var guid = Guid.Empty;

            var result = _browsingService.GetWebBrowserEnum(ref guid, out var browserFrameEnums);

            var frames = new IVsWindowFrame[1];

            while (ErrorHandler.Succeeded(result))
            {
                result = browserFrameEnums.Next(1, frames, out var fetchedCount);

                if (fetchedCount == 0)
                {
                    yield break;
                }
                else
                {
                    yield return frames[0];
                }
            }
        }
    }
}
