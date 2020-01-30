using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace DulcisX.Core.Models
{
    public class PropertiesX
    {
        private readonly Dictionary<PropertyType, string> _propertyCache;

        private readonly IVsHierarchy _vsHierarchy;

        private readonly HierarchyItem _hierarchyItem;

        private readonly IVsSolution _solution;

        private PropertiesX(HierarchyItem hierarchyItem)
                => _hierarchyItem = hierarchyItem;

        internal PropertiesX(IVsHierarchy vsHierarchy, HierarchyItem hierarchyItem) : this(hierarchyItem)
            => (_propertyCache, _vsHierarchy) = (new Dictionary<PropertyType, string>(), vsHierarchy);

        internal PropertiesX(IVsSolution solution) : this(HierarchyItem.Solution)
         => (_propertyCache, _solution) = (new Dictionary<PropertyType, string>(), solution);

        public string this[PropertyType type, bool forceRefresh = false]
        {
            get
            {
                if (!forceRefresh && _propertyCache.ContainsKey(type))
                {
                    return _propertyCache[type];
                }
                else
                {
                    switch (_hierarchyItem)
                    {
                        case HierarchyItem.Solution:
                            var (_, fileName, _) = GetSolutionProperties();

                            _propertyCache[PropertyType.FullName] = fileName;
                            break;
                        case HierarchyItem.Project:
                            var prop = GetProperty(type);

                            _propertyCache[type] = prop;
                            break;
                    }

                    return _propertyCache[type];
                }
            }
        }

        private string GetProperty(PropertyType type)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var propId = (int)type;

            var result = _vsHierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, propId, out var tempProp);

            VsHelper.ValidateVSStatusCode(result);

            return tempProp.ToString();
        }

        private (string dir, string fileName, string optionsPath) GetSolutionProperties()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = _solution.GetSolutionInfo(out var dir, out var fileName, out var optionsPath);

            VsHelper.ValidateVSStatusCode(result);

            return (dir, fileName, optionsPath);
        }
    }
}
