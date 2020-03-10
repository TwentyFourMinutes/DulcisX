using DulcisX.Core.Models;
using Microsoft.VisualStudio.Shell;
using SimpleInjector;

namespace DulcisX.Core.Extensions
{
    public static class ContainerExtensions
    {
        public static TCOMType GetCOMInstance<TCOMType>(this Container container)
            => container.GetInstance<COMContainer<TCOMType>>().Value;

        public static void RegisterCOMInstance<TService, TInterface>(this Container container, IServiceProviders providers) where TInterface : class
            => container.RegisterSingleton(() => COMContainer.Create(providers.GetService<TService, TInterface>()));
    }
}
