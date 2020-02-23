using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components
{
    public class ProjectX : HierarchyItemX
    {
        private Guid _underlyingGuid = Guid.Empty;

        public Guid UnderlyingGuid
        {
            get
            {
                if (_underlyingGuid == Guid.Empty)
                {
                    ThreadHelper.ThrowIfNotOnUIThread();

                    var result = ParentSolution.UnderlyingSolution.GetGuidOfProject(UnderlyingHierarchy, out _underlyingGuid);

                    VsHelper.ValidateSuccessStatusCode(result);
                }

                return _underlyingGuid;
            }
        }

        public IVsProject UnderlyingProject => (IVsProject)UnderlyingHierarchy;

        public __VSPROJOUTPUTTYPE OutputType
        {
            get => (__VSPROJOUTPUTTYPE)GetProperty((int)__VSHPROPID5.VSHPROPID_OutputType);
            set => SetProperty((int)__VSHPROPID5.VSHPROPID_OutputType, (uint)(int)value);
        }

        private IVsBuildPropertyStorage _vsBuildPropertyStorage;

        public IVsBuildPropertyStorage VsBuildPropertyStorage
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (_vsBuildPropertyStorage is null)
                {
                    _vsBuildPropertyStorage = (IVsBuildPropertyStorage)UnderlyingHierarchy;
                }

                return _vsBuildPropertyStorage;
            }
        }

        internal ProjectX(IVsHierarchy hierarchy, uint itemId, SolutionX solution, HierarchyItemX parentItem = null) : base(hierarchy, itemId, HierarchyItemTypeX.Project, ConstructorInstance.FromValue(solution), ConstructorInstance.Empty<ProjectX>(), parentItem)
        {

        }

        public string GetItemProperty(uint itemId, DocumentPropertyX documentProperty)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = VsBuildPropertyStorage.GetItemAttribute(itemId, documentProperty.ToString(), out var val);

            VsHelper.ValidateSuccessStatusCode(result);

            return val;
        }
    }
}
