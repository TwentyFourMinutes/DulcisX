using System;
using System.Linq;
using System.Reflection;

namespace DulcisX.Core
{
    internal sealed class ContainerConstructor
    {
        private readonly PackageX _package;

        private ContainerConstructor(PackageX package)
        {
            _package = package;
        }

        internal static ContainerConstructor Construct(PackageX package)
            => new ContainerConstructor(package);

        internal ContainerConstructor With(Assembly assembly)
        {
            if (assembly is null)
                return this;

            var configurations = assembly.GetTypes()
                                         .Where(x => x.IsClass && x.GetInterface(nameof(IContainerConfiguration)) != null);

            foreach (var configuration in configurations)
            {
                var configurationInstance = (IContainerConfiguration)Activator.CreateInstance(configuration);

                configurationInstance.ConfigureServices(_package, _package.ServiceContainer);
            }

            return this;
        }

        internal ContainerConstructor With(Assembly[] assemblies)
        {
            if (assemblies is null)
                return this;

            foreach (var assembly in assemblies)
            {
                With(assembly);
            }

            return this;
        }
    }
}
