using DulcisX.Helpers;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using DulcisX.Core.Extensions;

namespace DulcisX.Core.Models
{
    public abstract class HierarchyPropertiesX
    {
        public string FullName
        {
            get => GetProperty<string>((int)__VSHPROPID.VSHPROPID_Name);
            set => SetProperty((int)__VSHPROPID.VSHPROPID_Name, value);
        }

        public IVsHierarchy UnderlyingHierarchy { get; }

        public uint ItemId { get; }

        protected HierarchyPropertiesX(IVsHierarchy underlyingHierarchy, uint itemId)
            => (UnderlyingHierarchy, ItemId) = (underlyingHierarchy, itemId);

        protected TType GetProperty<TType>(int propId)
            => UnderlyingHierarchy.GetProperty<TType>(ItemId, propId);

        protected void SetProperty(int propId, object val)
            => UnderlyingHierarchy.SetProperty(ItemId, propId, val: val);
    }
}
