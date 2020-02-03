using DulcisX.Components.Events;
using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Core.Models.Interfaces;
using DulcisX.Helpers;
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

        public SelectedHierarchyItemsX SelectedHierarchyItems { get; }

        public IEnumerable<ProjectX> Projects => this;

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
                    _buildEvents = SolutionBuildEventsX.Create(this);
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

        internal SolutionX(IVsSolution solution, IServiceProviders providers) : base((IVsHierarchy)solution, null, VSConstants.VSITEMID_ROOT, HierarchyItemTypeX.Solution)
            => (UnderlyingSolution, ServiceProviders, SelectedHierarchyItems) = (solution, providers, new SelectedHierarchyItemsX(this));

        public ProjectX GetProject(Guid projectGuid)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return Projects.FirstOrDefault(x => x.UnderlyingGuid == projectGuid);
        }

        public ProjectX GetProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetGuidOfProject(hierarchy, out var projectGuid);
            VsHelper.ValidateVSStatusCode(result);

            return GetProject(projectGuid);
        }

        public new IEnumerator<ProjectX> GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var tempGuid = Guid.Empty;

            var result = UnderlyingSolution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, ref tempGuid, out var projectEnumerator);

            VsHelper.ValidateVSStatusCode(result);
            var hierarchy = new IVsHierarchy[1];

            while (true)
            {
                result = projectEnumerator.Next(1, hierarchy, out var success);

                if (success == 0)
                    break;

                VsHelper.ValidateVSStatusCode(result);

                yield return new ProjectX(hierarchy[0], VSConstants.VSITEMID_ROOT, this);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

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
    }
}
