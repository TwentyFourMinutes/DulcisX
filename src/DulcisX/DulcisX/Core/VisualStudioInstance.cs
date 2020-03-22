using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core
{
    public class VisualStudioInstance
    {
        private readonly IVsShell _shell;

        internal VisualStudioInstance(IVsShell shell)
        {
            _shell = shell;
        }

        public bool IsElevatedInstance()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = _shell as IVsShell3;

            var result = shell.IsRunningElevated(out var isElevated);

            ErrorHandler.ThrowOnFailure(result);

            return isElevated;
        }

        public void RestartInstance(bool restartAsElevated = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = _shell as IVsShell4;

            var result = shell.Restart(restartAsElevated ? (uint)__VSRESTARTTYPE.RESTART_Elevated : (uint)__VSRESTARTTYPE.RESTART_Normal);

            ErrorHandler.ThrowOnFailure(result);
        }

        public string GetInstallDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID.VSSPROPID_InstallDirectory, out var installDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)installDirObj;
        }

        public string GetProjectDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID.VSSPROPID_VisualStudioProjDir, out var projectDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)projectDirObj;
        }

        public string GetVisualStudioDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID2.VSSPROPID_VisualStudioDir, out var vsDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)vsDirObj;
        }

        public string GetLocalAppDataDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID4.VSSPROPID_LocalAppDataDir, out var localAppDataDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)localAppDataDirObj;
        }

        public string GetReleaseVersion()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID5.VSSPROPID_ReleaseVersion, out var releaseVersionObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)releaseVersionObj;
        }

        public string GetReleaseDescription()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID5.VSSPROPID_ReleaseDescription, out var releaseDescriptionObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)releaseDescriptionObj;
        }

        public bool IsPrereleaseVersion()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID7.VSSPROPID_IsPrerelease, out var prereleaseVersionObj);

            ErrorHandler.ThrowOnFailure(result);

            return (bool)prereleaseVersionObj;
        }
    }
}
