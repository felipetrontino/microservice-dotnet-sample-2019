using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Core.Common
{
    public static class Bootstrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config, Action<IServiceCollection, IConfiguration> register = null)
        {
            services.AddScoped<IConfiguration>(x => config);

            register?.Invoke(services, config);
        }
    }
}