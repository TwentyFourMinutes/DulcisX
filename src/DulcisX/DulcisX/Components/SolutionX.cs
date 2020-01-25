using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DulcisX.Components
{
    public class SolutionX : IEnumerable<ProjectX>
    {
        public IEnumerable<ProjectX> Projects => this;

        public IVsSolution UnderlyingSolution { get; }

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
    }
}
