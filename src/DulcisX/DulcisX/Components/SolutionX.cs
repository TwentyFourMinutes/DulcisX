using DulcisX.Components.Events;
using DulcisX.Core.Models.Interfaces;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DulcisX.Components
{
    public class SolutionX : BaseProjectItemX, IEnumerable<ProjectX>, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public IVsSolution UnderlyingSolution { get; }

        public IEnumerable<ProjectX> Projects => this;

        private SolutionEventsX _events;

        public SolutionEventsX SolutionEvents
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(SolutionX));

                if (_events is null)
                {
                    _events = SolutionEventsX.Create(this);
                }

                return _events;
            }
        }

        private SolutionBuildEventsX _buildEvents;

        public SolutionBuildEventsX SolutionBuidEvents
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(SolutionX));

                if (_buildEvents is null)
                {
                    _buildEvents = SolutionBuildEventsX.Create(this);
                }

                return _buildEvents;
            }
        }

        internal IServiceProviders ServiceProviders { get; }

        internal SolutionX(IVsSolution solution, IServiceProviders providers)
            => (UnderlyingSolution, ServiceProviders) = (solution, providers);

        public ProjectX GetProject(Guid projectGuid)
             => Projects.FirstOrDefault(x => x.UnderlyingGuid == projectGuid);

        public ProjectX GetProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingSolution.GetGuidOfProject(hierarchy, out var projectGuid);
            VsHelper.ValidateVSStatusCode(result);

            return GetProject(projectGuid);
        }

        public IEnumerator<ProjectX> GetEnumerator()
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

                yield return new ProjectX(hierarchy[0], this);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing && _events != null)
                {
                    SolutionEvents.Destroy(this);
                    SolutionBuidEvents.Destroy(this);
                }

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
