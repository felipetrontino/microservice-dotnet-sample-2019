using Book.Domain.Common;
using Book.Infra.Data;
using Framework.Core.Bus;
using Framework.Core.Bus.RabbitMQ;
using Framework.Core.Config;
using Framework.Data.EF;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Book.Infra.CrossCutting
{
    public static class BootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Infra
            services.AddMediatR(typeof(ContextNames).Assembly);
            services.AddSingleton<IBusConnection>(x => new BusConnection(config.GetConnectionString(ConnectionStringNames.Rabbit)));
            services.AddSingleton<IBusPublisher, BusPublisher>();
            services.AddSingleton<IBusReceiver, BusReceiver>();

            // Db
            services.AddDb<DbBook>(config.GetConnectionString(ConnectionStringNames.Sql));
        }
    }
}