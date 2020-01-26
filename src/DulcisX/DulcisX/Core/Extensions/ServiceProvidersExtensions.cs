using DulcisX.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DulcisX.Core.Extensions
{
    public static class ServiceProvidersExtensions
    {
        public static TService GetService<TService>(this IServiceProviders providers) where TService : class
            => GetService<TService, TService>(providers);

        public static TService GetService<TBaseService, TService>(this IServiceProviders providers) where TBaseService : class
                                                                                                    where TService : class
            => providers.GetService(typeof(TBaseService)) as TService;

        public static Task<TService> GetServiceAsync<TService>(this IServiceProviders providers) where TService : class
           => GetServiceAsync<TService, TService>(providers);

        public static async Task<TService> GetServiceAsync<TBaseService, TService>(this IServiceProviders providers) where TBaseService : class
                                                                                                                     where TService : class
            => (await providers.GetServiceAsync(typeof(TBaseService))) as TService;
    }
}
