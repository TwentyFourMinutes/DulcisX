using DulcisX.Core.Enums;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Core
{
    public class VisualStudioUIInstance
    {
        private readonly IVsUIShell _shell;

        internal VisualStudioUIInstance(IVsUIShell shell)
        {
            _shell = shell;
        }

        public MessageBoxResult ShowMessageBox(string title, string message, MessageBoxButton buttons, int selectedButtonIndex = 0, MessageBoxIcon icon = MessageBoxIcon.Info)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (selectedButtonIndex < 0 && selectedButtonIndex > 3)
            {
                throw new ArgumentException("The default selected button index can not be lower than 0 or greater than 3", nameof(selectedButtonIndex));
            }

            var emptyGuid = Guid.Empty;

            var result = _shell.ShowMessageBox(0u, ref emptyGuid, title, message, null, 0, (OLEMSGBUTTON)buttons, (OLEMSGDEFBUTTON)selectedButtonIndex, (OLEMSGICON)icon, 0, out var messageBoxResult);

            ErrorHandler.ThrowOnFailure(result);

            return (MessageBoxResult)messageBoxResult;
        }
    }
}
