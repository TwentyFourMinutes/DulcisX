using DulcisX.Core.Models;
using SimpleInjector;

namespace DulcisX.Core.Extensions
{
    public static class ContainerExtensions
    {
        public static TCOMType GetCOMInstance<TCOMType>(this Container container)
            => container.GetInstance<COMContainer<TCOMType>>().Value;
    }
}
