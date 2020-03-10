namespace DulcisX.Core.Models
{
    public class COMContainer<TComType>
    {
        public TComType Value { get; }

        internal COMContainer(TComType comType)
        {
            Value = comType;
        }
    }
    public static class COMContainer
    {
        public static COMContainer<TComType> Create<TComType>(TComType comType)
        {
            return new COMContainer<TComType>(comType);
        }
    }
}
