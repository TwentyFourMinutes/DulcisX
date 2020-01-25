using DulcisX.Components.Events;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DulcisX.Components
{
    public class SolutionX : IEnumerable<ProjectX>, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public IVsSolution UnderlyingSolution { get; }

        public IEnumerable<ProjectX> Projects => this;

        private SolutionEventsX _events;

        public SolutionEventsX Events
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(nameof(SolutionX));

                if (_events is null)
                {
                    ThreadHelper.ThrowIfNotOnUIThread();

                    _events = new SolutionEventsX(this);

                    var result = UnderlyingSolution.AdviseSolutionEvents(_events, out var cookieUID);

                    VsHelper.ValidateVSStatusCode(result);

                    _events.CookieUID = cookieUID;
                }

                return _events;
            }
        }

        public SolutionX(IVsSolution solution)
            => UnderlyingSolution = solution;

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

                yield return new ProjectX(hierarchy[0]);
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
                    ThreadHelper.ThrowIfNotOnUIThread();

                    var result = UnderlyingSolution.UnadviseSolutionEvents(Events.CookieUID);

                    VsHelper.ValidateVSStatusCode(result);
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
