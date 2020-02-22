using DulcisX.Components.Events;
using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Interfaces;
using DulcisX.Core.Models.PackageUserOptions;
using DulcisX.Helpers;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DulcisX.Components
{
    public class SolutionX : HierarchyItemX, IEnumerable<ProjectX>, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public IVsSolution UnderlyingSolution { get; }

        public IVsSolutionPersistence SolutionPersistence { get; }


        private IVsSolutionBuildManager _solutionBuildManager;

        public IVsSolutionBuildManager SolutionBuildManager
        {
            get
            {
                if (_solutionBuildManager is null)
                {
                    _solutionBuildManager = ServiceProviders.GetService<SVsSolutionBuildManager, IVsSolutionBuildManager>();
                }

                return _solutionBuildManager;
            }
        }


        #region Properties

        public IEnumerable<ProjectX> Projects => this;

        public IEnumerable<StartupProjectX> StartupProjects => GetStartupProjects();

        public SelectedHierarchyItemsX SelectedHierarchyItems { get; }

        #endregion

        #region Events

        private ISolutionEventsX _events;

        public ISolutionEventsX SolutionEvents
        {
            get
            {
                ThrowIfDisposed();

                if (_events is null)
                {
                    _events = SolutionEventsX.Create(this);
                }

                return _events;
            }
        }

        private ISolutionBuildEventsX _buildEvents;

        public ISolutionBuildEventsX SolutionBuidEvents
        {
            get
            {
                ThrowIfDisposed();

                if (_buildEvents is null)
                {
                    _buildEvents = SolutionBuildEventsX.Create(this, SolutionBuildManager);
                }

                return _buildEvents;
            }
        }

        private IRunningDocTableEventsX _documentEvents;

        public IRunningDocTableEventsX DocumentEvents
        {
            get
            {
                ThrowIfDisposed();

                if (_buildEvents is null)
                {
                    _documentEvents = RunningDocTableEventsX.Create(this);
                }

                return _documentEvents;
            }
        }

        #endregion

        internal IServiceProviders ServiceProviders { get; }

        internal DTE2 DTE2 { get; }

        internal SolutionX(IVsSolution solution, DTE2 dte2, IServiceProviders providers, IVsSolutionPersistence solutionPersistence) : base((IVsHierarchy)solution, VSConstants.VSITEMID_ROOT, HierarchyItemTypeX.Solution, ConstructorInstance.This<SolutionX>(), ConstructorInstance.Empty<ProjectX>())
            => (UnderlyingSolution, ServiceProviders, SelectedHierarchyItems, DTE2, SolutionPersistence) = (solution, providers, new SelectedHierarchyItemsX(this), dte2, solutionPersistence);

        #region Projects

        public ProjectX GetProject(string fullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var result = UnderlyingSolution.GetProjectOfUniqueName(fullName, out var hierarchy);
            VsHelper.ValidateSuccessStatusCode(result);

            return new ProjectX(hierarchy, VSConstants.VSITEMID_ROOT, this);
        }

        public ProjectX GetProject(Guid projectGuid)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return Projects.FirstOrDefault(x => x.UnderlyingGuid == projectGuid);
        }

        public ProjectX GetProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetGuidOfProject(hierarchy, out var projectGuid);
            VsHelper.ValidateSuccessStatusCode(result);

            return GetProject(projectGuid);
        }

        public new IEnumerator<ProjectX> GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var tempGuid = Guid.Empty;

            var result = UnderlyingSolution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, ref tempGuid, out var projectEnumerator);

            VsHelper.ValidateSuccessStatusCode(result);
            var hierarchy = new IVsHierarchy[1];

            while (true)
            {
                result = projectEnumerator.Next(1, hierarchy, out var success);

                if (success == 0)
                    break;

                VsHelper.ValidateSuccessStatusCode(result);

                yield return new ProjectX(hierarchy[0], VSConstants.VSITEMID_ROOT, this);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

        #region UserConfiguration

        public IEnumerable<StartupProjectX> GetStartupProjects()
        {
            var solutionConfiguration = new SolutionConfigurationOptions();

            GetUserConfiguration(solutionConfiguration, "SolutionConfiguration");

            if (solutionConfiguration.IsMultiStartup)
            {
                foreach (var startupProject in solutionConfiguration.StartupProjects)
                {
                    var project = GetProject(startupProject.Key);

                    yield return new StartupProjectX(startupProject.Value, project);
                }
            }
            else
            {
                var result = SolutionBuildManager.get_StartupProject(out var hierarchy);

                yield return new StartupProjectX(StartupOptions.Start, GetProject(hierarchy));

                VsHelper.ValidateSuccessStatusCode(result);
            }
        }

        public void GetUserConfiguration(IVsPersistSolutionOpts persistanceSolutionOptions, string streamKey)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = SolutionPersistence.LoadPackageUserOpts(persistanceSolutionOptions, streamKey);

            VsHelper.ValidateSuccessStatusCode(result);
        }

        #endregion

        #region Disposing

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing && _events != null)
                {
                    ((SolutionEventsX)SolutionEvents).Destroy(this);
                    ((SolutionBuildEventsX)SolutionBuidEvents).Destroy();
                    ((RunningDocTableEventsX)DocumentEvents).Destroy();
                }

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(SolutionX));
        }

        #endregion
    }
}
