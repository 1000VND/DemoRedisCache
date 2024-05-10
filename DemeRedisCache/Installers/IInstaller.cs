using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemeRedisCache.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
