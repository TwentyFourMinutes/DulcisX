using DulcisX.Components.Events;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;

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

                    _events = new SolutionEventsX();

                    UnderlyingSolution.AdviseSolutionEvents(_events, out var cookieUID);

                    _events.CookieUID = cookieUID;
                }

                return _events;
            }
        }

        public SolutionX(IVsSolution solution)
            => UnderlyingSolution = solution;

        public IEnumerator<ProjectX> GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var tempGuid = Guid.Empty;

            var result = UnderlyingSolution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, ref tempGuid, out var projectEnumerator);

            VSHelper.ValidateVSStatusCode(result);
            var hierarchy = new IVsHierarchy[1];

            while (true)
            {
                result = projectEnumerator.Next(1, hierarchy, out var success);

                if (success == 0)
                    break;

                VSHelper.ValidateVSStatusCode(result);

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

                    UnderlyingSolution.UnadviseSolutionEvents(Events.CookieUID);
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
