using DulcisX.Core.Components;
using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;
using System;

namespace DulcisX.Core
{
    public class VisualStudioUIInstance
    {
        private IVsStatusbar _statusBar;

        /// <summary>
        /// Gets the <see cref="IVsStatusbar"/> of the environment.
        /// </summary>
        public IVsStatusbar StatusBar
        {
            get
            {
                if (_statusBar is null)
                {
                    _statusBar = _serviceContainer.GetCOMInstance<IVsStatusbar>();
                }

                return _statusBar;
            }
        }

        private InfoBar _infoBar;

        /// <summary>
        /// Gets the InfoBar of the environment.
        /// </summary>
        public InfoBar InfoBar
        {
            get
            {
                if (_infoBar is null)
                {
                    _infoBar = new InfoBar(_serviceContainer.GetCOMInstance<IVsInfoBarUIFactory>(),
                                           _serviceContainer.GetCOMInstance<IVsInfoBarHost>(),
                                           WebBrowser);
                }

                return _infoBar;
            }
        }

        private WebBrowser _webBrowser;

        /// <summary>
        /// Gets the WebBrowser of the environment.
        /// </summary>
        public WebBrowser WebBrowser
        {
            get
            {
                if (_webBrowser is null)
                {
                    _webBrowser = new WebBrowser(_serviceContainer.GetCOMInstance<IVsWebBrowsingService>());
                }

                return _webBrowser;
            }
        }

        private readonly Container _serviceContainer;
        private readonly IVsUIShell _shell;

        internal VisualStudioUIInstance(Container container)
        {
            _serviceContainer = container;
            _shell = _serviceContainer.GetCOMInstance<IVsUIShell>();
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
