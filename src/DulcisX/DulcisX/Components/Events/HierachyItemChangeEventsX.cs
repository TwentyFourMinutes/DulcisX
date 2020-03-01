using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Linq;

namespace DulcisX.Components.Events
{
    public interface lols { }
    internal class HierachyItemChangeEventsX : BaseEventX, IVsFileChangeEvents, lols
    {
        private readonly IVsFileChangeEx _vsFileChangeEx;

        public HierachyItemChangeEventsX(SolutionX solution, IVsFileChangeEx vsFileChangeEx) : base(solution)
        {
            _vsFileChangeEx = vsFileChangeEx;
        }

        public int FilesChanged(uint cChanges, string[] rgpszFile, uint[] rggrfChange)
        {
            return VSConstants.S_OK;
        }

        public int DirectoryChanged(string pszDirectory)
        {
            return VSConstants.S_OK;
        }

        internal void Destroy()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = _vsFileChangeEx.UnadviseDirChange(CookieUID);
             ErrorHandler.ThrowOnFailure(result);
        }

        internal static HierachyItemChangeEventsX Create(SolutionX solution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var fileChange = solution.ServiceProviders.GetService<SVsFileChangeEx, IVsFileChangeEx>();

            var hierachyItemChangeEvents = new HierachyItemChangeEventsX(solution, fileChange);

            var folder = solution.Projects.First().Skip(1).Take(1).First();
            var result = fileChange.AdviseDirChange(folder.FullName, VSConstants.S_FALSE, hierachyItemChangeEvents, out var cookieUID);

             ErrorHandler.ThrowOnFailure(result);

            hierachyItemChangeEvents.CookieUID = cookieUID;

            return hierachyItemChangeEvents;
        }
    }
}
