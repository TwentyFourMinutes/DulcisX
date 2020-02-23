using Microsoft.VisualStudio.Shell.Interop;
using DulcisX.Core.Extensions;

namespace DulcisX.Core.Models
{
    public abstract class HierarchyPropertiesX
    {
        public string Name
        {
            get => GetProperty<string>((int)__VSHPROPID.VSHPROPID_Name);
            set => SetProperty((int)__VSHPROPID.VSHPROPID_Name, value);
        }

        public IVsHierarchy UnderlyingHierarchy { get; }

        public uint ItemId { get; }

        protected HierarchyPropertiesX(IVsHierarchy parentHierarchy, uint itemId)
            => (UnderlyingHierarchy, ItemId) = (parentHierarchy, itemId);

        #region Get/Set Properties

        public uint GetProperty(int propId)
            => UnderlyingHierarchy.GetProperty(ItemId, propId);

        public TType GetProperty<TType>(int propId)
            => UnderlyingHierarchy.GetProperty<TType>(ItemId, propId);

        public object GetPropertyObject(int propId)
            => UnderlyingHierarchy.GetPropertyObject(ItemId, propId);

        public bool TryGetProperty(int propId, out uint value)
            => UnderlyingHierarchy.TryGetProperty(ItemId, propId, out value);

        public bool TryGetProperty<TType>(int propId, out TType type)
            => UnderlyingHierarchy.TryGetProperty(ItemId, propId, out type);

        public bool TryGetPropertyObject(int propId, out object obj)
            => UnderlyingHierarchy.TryGetPropertyObject(ItemId, propId, out obj);

        public void SetProperty(int propId, object val)
            => UnderlyingHierarchy.SetProperty(ItemId, propId, val);

        public bool TrySetProperty(int propId, object val)
            => UnderlyingHierarchy.TrySetProperty(ItemId, propId, val);

        #endregion
    }
}
