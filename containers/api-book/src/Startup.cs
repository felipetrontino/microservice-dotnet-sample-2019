using Book.Infra.CrossCutting;
using Book.Infra.Data;
using Framework.Core.Config;
using Framework.Web.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Book.Api
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void RegisterService(IServiceCollection services, IConfiguration config)
        {
            BootStrapper.RegisterServices(services, config);
            services.AddDbContext<DbBook>(opt => opt.UseSqlServer(config.GetConnectionString(ConnectionStringNames.Sql), x => x.UseNetTopologySuite()));

            services.AddHealthChecks()
                .AddRabbitMQ(rabbitMQConnectionString: config.GetConnectionString(ConnectionStringNames.Rabbit))
                .AddSqlServer(config.GetConnectionString(ConnectionStringNames.Sql));
        }
    }
}