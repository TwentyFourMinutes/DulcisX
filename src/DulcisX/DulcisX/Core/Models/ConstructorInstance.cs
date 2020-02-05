using DulcisX.Components;
using System.Threading.Tasks;

namespace DulcisX.Core.Models
{
    internal static class ConstructorInstance
    {
        internal static ConstructorInstance<TType> This<TType>() where TType : HierarchyItemX
            => new ConstructorInstance<TType>(false);

        internal static ConstructorInstance<TType> FromValue<TType>(TType val) where TType : HierarchyItemX
            => new ConstructorInstance<TType>(val);

        internal static ConstructorInstance<TType> Empty<TType>() where TType : HierarchyItemX
            => new ConstructorInstance<TType>(true);
    }

    internal class ConstructorInstance<TType> where TType : HierarchyItemX
    {
        private readonly TType _value;
        private readonly bool _isEmpty;

        internal ConstructorInstance(bool isEmpty)
            => _isEmpty = isEmpty;

        internal ConstructorInstance(TType value) : this(value is null)
            => _value = value;

        internal TType GetInstance(HierarchyItemX baseInstance)
        {
            if (_isEmpty)
                return null;
            else
                return _value ?? (TType)baseInstance;
        }
    }
}
