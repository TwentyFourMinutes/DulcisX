using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DulcisX.Core.Models
{
    public class PropertiesX
    {
        private readonly Dictionary<PropertyType, string> _propertyCache;

        private readonly IVsHierarchy _vsHierarchy;

        private readonly HierarchyItem _hierarchyItem;

        internal PropertiesX(IVsHierarchy vsHierarchy, HierarchyItem hierarchyItem = HierarchyItem.RootItem)
            => (_propertyCache, _vsHierarchy, _hierarchyItem) = (new Dictionary<PropertyType, string>(), vsHierarchy, hierarchyItem);

        public string this[PropertyType type, bool forceRefresh = false]
        {
            get
            {
                if (!forceRefresh && _propertyCache.ContainsKey(type))
                {
                    return _propertyCache[type];
                }
                else if (forceRefresh)
                {
                    var prop = GetProperty(type);

                    _propertyCache[type] = prop;

                    return prop;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        private string GetProperty(PropertyType type)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            var propId = (int)type;

            var result = _vsHierarchy.GetProperty((uint)_hierarchyItem, propId, out object property);

            VsHelper.ValidateVSStatusCode(result);

            return property.ToString();
        }
    }
}
