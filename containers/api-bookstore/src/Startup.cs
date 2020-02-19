using Bookstore.Data;
using Bookstore.Infra.CrossCutting;
using Framework.Core.Config;
using Framework.Web.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Api
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
            services.AddDbContext<DbBookstore>(opt => opt.UseSqlServer(config.GetConnectionString(ConnectionStringNames.Sql), x => x.UseNetTopologySuite()));

            services.AddHealthChecks()
                .AddRabbitMQ(rabbitMQConnectionString: config.GetConnectionString(ConnectionStringNames.Rabbit))
                .AddSqlServer(config.GetConnectionString(ConnectionStringNames.Sql));
        }
    }
}