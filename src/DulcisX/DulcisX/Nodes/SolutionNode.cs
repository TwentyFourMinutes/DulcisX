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
using DulcisX.Core.Models;
using System.IO;

namespace DulcisX.Nodes
{
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

        #endregion

        public SelectedNodes SelectedNodes { get; }

        public IVsSolution UnderlyingSolution { get; }

        public override NodeTypes NodeType => NodeTypes.Solution;

        public override SolutionNode ParentSolution => this;

        public Container ServiceContainer { get; }

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

        public SolutionNode(IVsSolution solution, IServiceProviders serviceProviders, Container container) : base(null, (IVsHierarchy)solution, CommonNodeIds.Solution)
        {
            SelectedNodes = new SelectedNodes(this);

            UnderlyingSolution = solution;
            ServiceProviders = serviceProviders;
            ServiceContainer = container;
        }

        public string GetFileName()
            => Path.GetFileName(GetFullName());

        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return (string)fullName;
        }

        public override BaseNode GetParent()
            => null;

        public override BaseNode GetParent(NodeTypes nodeType)
            => null;

        #region Project

        public ProjectNode GetProject(string uniqueName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProjectOfUniqueName(uniqueName, out var hierarchy);
            ErrorHandler.ThrowOnFailure(result);

            return new ProjectNode(this, hierarchy);
        }

        public ProjectNode GetProject(Guid projectGuid)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProjectOfGuid(projectGuid, out var hierarchy);
            ErrorHandler.ThrowOnFailure(result);

            return new ProjectNode(this, hierarchy);
        }

        public ProjectNode GetProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return new ProjectNode(this, hierarchy);
        }

        public IEnumerable<(ProjectNode Project, StartupOptions Options)> GetStartupProjects()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionConfiguration = new SolutionConfigurationOptions();

            GetUserConfiguration(solutionConfiguration, CommonStreamKeys.SolutionConfiguration);

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

                yield return (GetProject(hierarchy), StartupOptions.Start);

                ErrorHandler.ThrowOnFailure(result);
            }
        }

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

        public void GetUserConfiguration(IVsPersistSolutionOpts persistanceSolutionOptions, string streamKey)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionPersistance = ServiceContainer.GetCOMInstance<IVsSolutionPersistence>();

            var result = solutionPersistance.LoadPackageUserOpts(persistanceSolutionOptions, streamKey);

            ErrorHandler.ThrowOnFailure(result);
        }

        #endregion

        public bool IsSolutionFullyLoaded()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID4.VSPROPID_IsSolutionFullyLoaded, out var isLoaded);

            ErrorHandler.ThrowOnFailure(result);

            return Unbox.AsBoolean(isLoaded);
        }

        public bool IsTempSolution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out _);

            return result == VSConstants.E_UNEXPECTED;
        }
    }
}
