using SimpleInjector;

namespace DulcisX.Core
{
    /// <summary>
    /// Allows the configuration of the <see cref="PackageX.ServiceContainer"/>.
    /// </summary>
    public interface IContainerConfiguration
    {
        /// <summary>
        /// Configures the services of the see <see cref="PackageX.ServiceContainer"/>.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="container"></param>
        void ConfigureServices(PackageX package, Container container);
    }
}
