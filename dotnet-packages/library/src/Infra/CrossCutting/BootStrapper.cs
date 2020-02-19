using Framework.Core.Bus;
using Framework.Core.Bus.RabbitMQ;
using Framework.Core.Config;
using Framework.Data.EF;
using Library.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infra.CrossCutting
{
    public static class BootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Infra
            services.AddMediatR(typeof(BootStrapper).Assembly);
            services.AddSingleton<IBusConnection>(x => new BusConnection(config.GetConnectionString(ConnectionStringNames.Rabbit)));
            services.AddSingleton<IBusPublisher, BusPublisher>();
            services.AddSingleton<IBusReceiver, BusReceiver>();

            // Db
            services.AddDb<DbLibrary>(config.GetConnectionString(ConnectionStringNames.Sql));
        }
    }
}