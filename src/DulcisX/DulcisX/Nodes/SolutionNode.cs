using DulcisX.Core.Extensions;
using DulcisX.Core.Enums;
using DulcisX.Core.Enums.VisualStudio;
using DulcisX.Nodes.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SimpleInjector;
using System;
using System.Collections.Generic;
using DulcisX.Core.Components;
using System.IO;
using DulcisX.Core;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a Solution, which is the root of Node of the Solution Explorer.
    /// </summary>
    public class SolutionNode : SolutionItemNode, IPhysicalNode
    {
        #region Events

        private ISolutionEvents _solutionEvents;

        public ISolutionEvents SolutionEvents
            => _solutionEvents ?? (_solutionEvents = ServiceContainer.GetInstance<ISolutionEvents>());

        private ISolutionBuildEvents _solutionBuildEvents;

        public ISolutionBuildEvents SolutionBuildEvents
            => _solutionBuildEvents ?? (_solutionBuildEvents = ServiceContainer.GetInstance<ISolutionBuildEvents>());

        private IOpenNodeEvents _openNodeEvents;

        public IOpenNodeEvents OpenNodeEvents
            => _openNodeEvents ?? (_openNodeEvents = ServiceContainer.GetInstance<IOpenNodeEvents>());

        private INodeSelectionEvents _nodeSelectionEvents;

        public INodeSelectionEvents NodeSelectionEvents
            => _nodeSelectionEvents ?? (_nodeSelectionEvents = ServiceContainer.GetInstance<INodeSelectionEvents>());

        private IProjectNodeChangeEvents _projectNodeChangedEvents;

        public IProjectNodeChangeEvents ProjectNodeChangedEvents
            => _projectNodeChangedEvents ?? (_projectNodeChangedEvents = ServiceContainer.GetInstance<IProjectNodeChangeEvents>());

        #endregion

        /// <summary>
        /// Gets a <see cref="SelectedNodes"/> instance for the current Solution Explorer.
        /// </summary>
        public SelectedNodesCollection SelectedNodes { get; }

        /// <summary>
        /// Gets the native <see cref="IVsSolution"/> for the current <see cref="SolutionNode"/>.
        /// </summary>
        public IVsSolution UnderlyingSolution { get; }

        /// <inheritdoc/>
        public override NodeTypes NodeType => NodeTypes.Solution;

        /// <inheritdoc/>
        public override SolutionNode ParentSolution => this;

        /// <summary>
        /// Gets the <see cref="Container"/> which holds package and user specifc services.
        /// </summary>
        public Container ServiceContainer { get; }

        /// <summary>
        /// Gets the <see cref="IServiceProviders"/> which hold the environments services.
        /// </summary>
        public IServiceProviders ServiceProviders { get; }

        private IVsRunningDocumentTable _runningDocumentTable;

        internal IVsRunningDocumentTable RunningDocumentTable
        {
            get
            {
                if (_runningDocumentTable is null)
                {
                    _runningDocumentTable = ServiceContainer.GetCOMInstance<IVsRunningDocumentTable>();
                }

                return _runningDocumentTable;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionNode"/> class.
        /// </summary>
        /// <param name="solution">The native representation of the Solution.</param>
        /// <param name="serviceProviders">The environments services</param>
        /// <param name="container">A Conainer which holds package and user specifc services.</param>
        public SolutionNode(IVsSolution solution, IServiceProviders serviceProviders, Container container) : base(null, (IVsHierarchy)solution, CommonNodeIds.Solution)
        {
            SelectedNodes = new SelectedNodesCollection(this);

            UnderlyingSolution = solution;
            ServiceProviders = serviceProviders;
            ServiceContainer = container;
        }

        /// <inheritdoc/>
        public string GetFileName()
            => Path.GetFileName(GetFullName());

        /// <inheritdoc/>
        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return (string)fullName;
        }

        /// <inheritdoc/>
        /// <remarks>This method will always return null. A <see cref="SolutionNode"/> can't have any parents.</remarks>
        public override BaseNode GetParent()
            => null;

        /// <inheritdoc/>
        /// <remarks>This method will always return null. A <see cref="SolutionNode"/> can't have any parents.</remarks>
        public override BaseNode GetParent(NodeTypes nodeType)
            => null;

        #region Project

        /// <summary>
        /// Returns a new <see cref="ProjectNode"/> instance given the <paramref name="uniqueName"/> of the project.
        /// </summary>
        /// <param name="uniqueName">The unique name, aka. the full name, of the project.</param>
        /// <returns>A new <see cref="ProjectNode"/> instance.</returns>
        public ProjectNode GetProject(string uniqueName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProjectOfUniqueName(uniqueName, out var hierarchy);
            ErrorHandler.ThrowOnFailure(result);

            return new ProjectNode(this, hierarchy);
        }

        /// <summary>
        /// Returns a new <see cref="ProjectNode"/> instance given the <paramref name="projectGuid"/> of the project.
        /// </summary>
        /// <param name="projectGuid">The guid of the project.</param>
        /// <returns>A new <see cref="ProjectNode"/> instance.</returns>
        public ProjectNode GetProject(Guid projectGuid)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProjectOfGuid(projectGuid, out var hierarchy);
            ErrorHandler.ThrowOnFailure(result);

            return new ProjectNode(this, hierarchy);
        }

        /// <summary>
        /// Returns a new <see cref="ProjectNode"/> instance given the <paramref name="hierarchy"/> of the project.
        /// </summary>
        /// <param name="hierarchy">The hierarchy of the project.</param>
        /// <returns>A new <see cref="ProjectNode"/> instance.</returns>
        public ProjectNode GetProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return new ProjectNode(this, hierarchy);
        }

        /// <summary>
        /// Returns all startup projects in the current <see cref="SolutionNode"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all startup projects.</returns>
        public IEnumerable<(ProjectNode Project, StartupOption Options)> GetStartupProjects()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionConfiguration = new SolutionConfigurationOptions();

            LoadUserConfiguration(solutionConfiguration, CommonStreamKeys.SolutionConfiguration);

            if (solutionConfiguration.IsMultiStartup)
            {
                foreach (var startupProject in solutionConfiguration.StartupProjects)
                {
                    var project = GetProject(startupProject.Key);

                    yield return (project, startupProject.Value);
                }
            }
            else
            {
                var solutionBuildManager = ServiceContainer.GetCOMInstance<IVsSolutionBuildManager>();

                var result = solutionBuildManager.get_StartupProject(out var hierarchy);

                yield return (GetProject(hierarchy), StartupOption.Start);

                ErrorHandler.ThrowOnFailure(result);
            }
        }

        /// <summary>
        /// Returns all projects in the current <see cref="SolutionNode"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ProjectNode}"/> containing all projects.</returns>
        public IEnumerable<ProjectNode> GetAllProjects()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var tempGuid = Guid.Empty;

            var result = UnderlyingSolution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_ALLPROJECTS, ref tempGuid, out var projectEnumerator);

            ErrorHandler.ThrowOnFailure(result);

            var hierarchy = new IVsHierarchy[1];

            while (true)
            {
                result = projectEnumerator.Next(1, hierarchy, out var fetchedCount);

                if (fetchedCount == 0)
                    break;

                ErrorHandler.ThrowOnFailure(result);

                if (!(NodeFactory.GetSolutionItemNode(this, hierarchy[0], CommonNodeIds.Project) is ProjectNode projectNode))
                {
                    continue;
                }

                yield return projectNode;
            }
        }

        /// <summary>
        /// Loads the user configuration specified in the .suo file.
        /// </summary>
        /// <param name="persistanceSolutionOptions">The <see cref="IVsPersistSolutionOpts"/> instance which handels all persistance operations.</param>
        /// <param name="streamKey">The identifier for the stream to load. Usually any of the values in the <see cref="CommonStreamKeys"/> class.</param>
        public void LoadUserConfiguration(IVsPersistSolutionOpts persistanceSolutionOptions, string streamKey)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionPersistance = ServiceContainer.GetCOMInstance<IVsSolutionPersistence>();

            var result = solutionPersistance.LoadPackageUserOpts(persistanceSolutionOptions, streamKey);

            ErrorHandler.ThrowOnFailure(result);
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the current <see cref="SolutionNode"/> is fully loaded.
        /// </summary>
        /// <returns><see langword="true"/> if the Solution is fully loaded; otherwise <see langword="false"/>.</returns>
        public bool IsSolutionFullyLoaded()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID4.VSPROPID_IsSolutionFullyLoaded, out var isLoaded);

            ErrorHandler.ThrowOnFailure(result);

            return Unbox.AsBoolean(isLoaded);
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="SolutionNode"/> is a temporary Solution.
        /// </summary>
        /// <returns><see langword="true"/> if the Solution is temporary; otherwise <see langword="false"/>.</returns>
        public bool IsTempSolution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out _);

            return result == VSConstants.E_UNEXPECTED;
        }

        /// <summary>
        /// Saves the solution file and all children within the current <see cref="SolutionNode"/>.
        /// </summary>
        /// <param name="forceSave">Determines whether to force the file save operation or not.</param>
        public void SaveAllChildren(bool forceSave = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.SaveSolutionElement(forceSave ? (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave : (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_SaveIfDirty, null, 0);

            ErrorHandler.ThrowOnFailure(result);
        }
    }
}
