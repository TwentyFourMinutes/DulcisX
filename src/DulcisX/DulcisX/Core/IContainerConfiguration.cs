using SimpleInjector;

namespace DulcisX.Core
{
    public interface IContainerConfiguration
    {
        void ConfigureServices(PackageX package, Container container);
    }
}
