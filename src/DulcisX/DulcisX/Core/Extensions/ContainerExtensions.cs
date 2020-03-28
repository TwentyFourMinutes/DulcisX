using Microsoft.VisualStudio.Shell;
using SimpleInjector;

namespace DulcisX.Core.Extensions
{
    /// <summary>
    /// <see cref="Container"/> specific Extensions.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Returns a Com service which is register in the <see cref="PackageX.ServiceContainer"/>.
        /// </summary>
        /// <typeparam name="TCOMType">The wrapped Com Type service which should be retrieved.</typeparam>
        /// <param name="container">The <see cref="Container"/> from which the Com service should be retrieved.</param>
        /// <returns>The service being requested if available, otherwise null.</returns>
        public static TCOMType GetCOMInstance<TCOMType>(this Container container)
            => container.GetInstance<ComContainer<TCOMType>>().Value;

        /// <summary>
        /// Registers a <see cref="ComContainer{TComType}"/> in the <see cref="PackageX.ServiceContainer"/>.
        /// </summary>
        /// <typeparam name="TService">The Com service type of the implemention which should be stored.</typeparam>
        /// <typeparam name="TInterface">The implemention type of the Com service which should be retrieved.</typeparam>
        /// <param name="container">The <see cref="Container"/> in which the Com service will be registered.</param>
        /// <param name="providers">The native <see cref="IServiceProviders"/> provided by the environment implemented by the <see cref="PackageX"/>.</param>
        public static void RegisterCOMInstance<TService, TInterface>(this Container container, IServiceProviders providers) where TInterface : class
            => container.RegisterSingleton(() => ComContainer.Create(providers.GetService<TService, TInterface>()));
    }
}
