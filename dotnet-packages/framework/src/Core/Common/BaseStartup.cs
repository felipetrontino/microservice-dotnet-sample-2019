using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Common
{
    public abstract class BaseStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            RegisterService(services, config);
        }

        protected virtual void RegisterService(IServiceCollection services, IConfiguration config)
        {
        }
    }
}