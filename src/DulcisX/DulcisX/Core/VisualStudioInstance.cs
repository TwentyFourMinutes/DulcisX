using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DulcisX.Core
{
    /// <summary>
    /// Represents the current Visual Studio Instance.
    /// </summary>
    public class VisualStudioInstance
    {
        private readonly IVsShell _shell;

        internal VisualStudioInstance(IVsShell shell)
        {
            _shell = shell;
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="VisualStudioInstance"/> is running as an elevated process.
        /// </summary>
        /// <returns><see langword="true"/> if the process is elevated; otherwise <see langword="false"/>.</returns>
        public bool IsElevatedInstance()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = _shell as IVsShell3;

            var result = shell.IsRunningElevated(out var isElevated);

            ErrorHandler.ThrowOnFailure(result);

            return isElevated;
        }

        /// <summary>
        /// Restarts the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <param name="restartAsElevated">Determines whether the new instance should be elevated or not.</param>
        public void RestartInstance(bool restartAsElevated = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = _shell as IVsShell4;

            var result = shell.Restart(restartAsElevated ? (uint)__VSRESTARTTYPE.RESTART_Elevated : (uint)__VSRESTARTTYPE.RESTART_Normal);

            ErrorHandler.ThrowOnFailure(result);
        }

        /// <summary>
        /// Gets the Install Directory of the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <returns>A string that contains the absolute path of the Install Directory.</returns>
        public string GetInstallDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID.VSSPROPID_InstallDirectory, out var installDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)installDirObj;
        }

        /// <summary>
        /// Gets the Project Directory of the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <returns>A string that contains the absolute path of the Project Directory.</returns>
        public string GetProjectDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID.VSSPROPID_VisualStudioProjDir, out var projectDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)projectDirObj;
        }

        /// <summary>
        /// Gets the Visual Studio Directory of the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <returns>A string that contains the absolute path of the Visual Studio Directory.</returns>
        public string GetVisualStudioDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID2.VSSPROPID_VisualStudioDir, out var vsDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)vsDirObj;
        }

        /// <summary>
        /// Gets the Local AppData Directory of the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <returns>A string that contains the absolute path of the Local AppData Directory.</returns>
        public string GetLocalAppDataDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID4.VSSPROPID_LocalAppDataDir, out var localAppDataDirObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)localAppDataDirObj;
        }

        /// <summary>
        /// Gets the Release Version of the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <returns>A string that contains the Release Version.</returns>
        public string GetReleaseVersion()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID5.VSSPROPID_ReleaseVersion, out var releaseVersionObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)releaseVersionObj;
        }

        /// <summary>
        /// Gets the Release Description of the current <see cref="VisualStudioInstance"/>.
        /// </summary>
        /// <returns>A string that contains the Release Description.</returns>
        public string GetReleaseDescription()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID5.VSSPROPID_ReleaseDescription, out var releaseDescriptionObj);

            ErrorHandler.ThrowOnFailure(result);

            return (string)releaseDescriptionObj;
        }

        /// <summary>
        /// Gets a value indicating whether the version of the current <see cref="VisualStudioInstance"/> is a pre-release version.
        /// </summary>
        /// <returns><see langword="true"/> if the instance a pre-release version; otherwise <see langword="false"/>.</returns>
        public bool IsPrereleaseVersion()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _shell.GetProperty((int)__VSSPROPID7.VSSPROPID_IsPrerelease, out var prereleaseVersionObj);

            ErrorHandler.ThrowOnFailure(result);

            return (bool)prereleaseVersionObj;
        }
    }
}
