using DulcisX.Core.Extensions;
using DulcisX.Helpers;
using DulcisX.Nodes;
using DulcisX.Nodes.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using SimpleInjector;
using System.IO;

namespace DulcisX.Core
{
    public class SolutionExplorer
    {
        #region Events

        private ISolutionEvents _solutionEvents;

        /// <summary>
        /// Provides access to a <see cref="IVsSolutionEvents"/> instance.
        /// </summary>
        public ISolutionEvents SolutionEvents
            => _solutionEvents ?? (_solutionEvents = _serviceContainer.GetInstance<ISolutionEvents>());

        private ISolutionBuildEvents _solutionBuildEvents;

        /// <summary>
        /// Provides access to a <see cref="IVsSolutionEvents"/> instance.
        /// </summary>
        public ISolutionBuildEvents SolutionBuildEvents
            => _solutionBuildEvents ?? (_solutionBuildEvents = _serviceContainer.GetInstance<ISolutionBuildEvents>());

        private IOpenNodeEvents _openNodeEvents;

        /// <summary>
        /// Provides access to a <see cref="IOpenNodeEvents"/> instance.
        /// </summary>
        public IOpenNodeEvents OpenNodeEvents
            => _openNodeEvents ?? (_openNodeEvents = _serviceContainer.GetInstance<IOpenNodeEvents>());

        private INodeSelectionEvents _nodeSelectionEvents;

        /// <summary>
        /// Provides access to a <see cref="INodeSelectionEvents"/> instance.
        /// </summary>
        public INodeSelectionEvents NodeSelectionEvents
            => _nodeSelectionEvents ?? (_nodeSelectionEvents = _serviceContainer.GetInstance<INodeSelectionEvents>());

        private IProjectNodeChangeEvents _projectNodeChangedEvents;

        /// <summary>
        /// Provides access to a <see cref="IProjectNodeChangeEvents"/> instance.
        /// </summary>
        public IProjectNodeChangeEvents ProjectNodeChangedEvents
            => _projectNodeChangedEvents ?? (_projectNodeChangedEvents = _serviceContainer.GetInstance<IProjectNodeChangeEvents>());

        #endregion

        private SelectedNodesCollection _selectedNodes;

        /// <summary>
        /// Gets a <see cref="SelectedNodes"/> instance for the current Solution Explorer.
        /// </summary>
        public SelectedNodesCollection SelectedNodes
        {
            get
            {
                if (_selectedNodes is null)
                {
                    _selectedNodes = new SelectedNodesCollection(Solution);
                }

                return _selectedNodes;
            }
        }

        private SolutionNode _solution;

        /// <summary>
        /// Gets the currently open Solution.
        /// </summary>
        public SolutionNode Solution
        {
            get
            {
                if (_solution is null)
                {
                    _solution = new SolutionNode(_solutionBase, _serviceContainer.GetInstance<IServiceProviders>(), _serviceContainer);
                }

                return _solution;
            }
        }


        private readonly Container _serviceContainer;
        private readonly IVsSolution _solutionBase;

        internal SolutionExplorer(Container container)
        {
            _serviceContainer = container;
            _solutionBase = _serviceContainer.GetCOMInstance<IVsSolution>();
        }

        public bool IsInOpenFolderMode()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _solutionBase.GetProperty((int)__VSPROPID7.VSPROPID_IsInOpenFolderMode, out var isOpenFolderModeObj);

            ErrorHandler.ThrowOnFailure(result);

            return (bool)isOpenFolderModeObj;
        }

        public void OpenSolution(string fullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!File.Exists(fullName))
            {
                throw new FileNotFoundException("The specified soltion could not be found.", fullName);
            }

            var result = _solutionBase.OpenSolutionFile(0, fullName);

            ErrorHandler.ThrowOnFailure(result);
        }

        public bool TryOpenSolutionWithDialog(string startDirectory, bool solutionAsFilter = true)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!Directory.Exists(startDirectory))
            {
                throw new DirectoryNotFoundException("The specified directory could not be found.");
            }

            var result = _solutionBase.OpenSolutionViaDlg(startDirectory, VsConverter.FromBoolean(solutionAsFilter));

            return ErrorHandler.Succeeded(result);
        }

        public bool TryOpenSolutionWithDialog(OpenFileDialog fileDialog)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var success = fileDialog.ShowDialog();

            if (!success.HasValue || !success.Value)
            {
                return false;
            }

            if (!File.Exists(fileDialog.FileName))
            {
                throw new FileNotFoundException("The specified file could not be found.", fileDialog.FileName);
            }

            var result = _solutionBase.OpenSolutionFile(0, fileDialog.FileName);

            return ErrorHandler.Succeeded(result);
        }

        public void CloseSolution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _solutionBase.CloseSolutionElement((uint)__VSSLNCLOSEOPTIONS.SLNCLOSEOPT_SLNSAVEOPT_MASK, null, 0);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}
