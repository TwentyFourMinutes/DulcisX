using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components
{
    public class ProjectX : BaseSolutionItemX
    {
        private Guid _underlyingGuid = Guid.Empty;

        public Guid UnderlyingGuid
        {
            get
            {
                if (_underlyingGuid == Guid.Empty)
                {
                    Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

                    var result = Solution.UnderlyingSolution.GetGuidOfProject(UnderlyingHierarchy, out _underlyingGuid);

                    VsHelper.ValidateVSStatusCode(result);
                }

                return _underlyingGuid;
            }
        }

        public IVsHierarchy UnderlyingHierarchy { get; }

        //public string HierarchyItems
        //{
        //    get
        //    {
        //        VSConstants.VSITEMID
        //        UnderlyingHierarchy.
        //    }
        //}

        public SolutionX Solution { get; }

        public ProjectX(IVsHierarchy hierarchy, SolutionX solution) : base(new PropertiesX(hierarchy, HierarchyItem.Project))
            => (UnderlyingHierarchy, Solution) = (hierarchy, solution);
    }
}
