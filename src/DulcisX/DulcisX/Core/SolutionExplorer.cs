using DulcisX.Core.Extensions;
using DulcisX.Nodes;
using DulcisX.Nodes.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;

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
    }
}
