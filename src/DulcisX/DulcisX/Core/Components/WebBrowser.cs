using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Core.Components
{
    /// <summary>
    /// Wraps around the <see cref="IVsWebBrowsingService"/>.
    /// </summary>
    public class WebBrowser
    {
        private readonly IVsWebBrowsingService _browsingService;

        internal WebBrowser(IVsWebBrowsingService browsingService)
        {
            _browsingService = browsingService;
        }

        /// <summary>
        /// Opens a Browser window with the given <see cref="Uri"/> from wihtin a Visual Studio Browser.
        /// </summary>
        /// <param name="uri">The uri which should be opened in the Browser.</param>
        /// <param name="flags">The navigation options.</param>
        /// <returns>A new <see cref="IVsWindowFrame"/> instance pointing to the newly created Browser window.</returns>
        public IVsWindowFrame OpenInternal(Uri uri, __VSWBNAVIGATEFLAGS flags = 0)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _browsingService.Navigate(uri.AbsoluteUri, (uint)flags, out var frame);

            ErrorHandler.ThrowOnFailure(result);

            return frame;
        }

        /// <summary>
        /// Opens a Browser tab in the default system Brwoser with the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The uri which should be opened in the Browser.</param>
        /// <param name="creationFlags">The creation options.</param>
        /// <param name="previewFlags">The display options.</param>
        public void OpenExternal(Uri uri, __VSCREATEWEBBROWSER creationFlags = __VSCREATEWEBBROWSER.VSCWB_ForceNew, VSPREVIEWRESOLUTION previewFlags = VSPREVIEWRESOLUTION.PR_Default)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _browsingService.CreateExternalWebBrowser((uint)creationFlags, previewFlags, uri.AbsoluteUri);

            ErrorHandler.ThrowOnFailure(result);
        }

        /// <summary>
        /// Gets all Browser window pointers which are opened in Visual Studio.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{IVsWindowFrame}"/> containg the opened Browser windows.</returns>
        public IEnumerable<IVsWindowFrame> GetInternalBrowserWindows()
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
