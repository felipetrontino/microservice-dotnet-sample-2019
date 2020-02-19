using Book.Tools.DbContext;
using Framework.Core.Config;
using Framework.Core.Job.TaskRunner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Book.Tools.Tasks
{
    public class MigrationTaskRunner : BaseTask
    {
        protected override void Configure(IServiceCollection services, IConfiguration config)
        {
            var conn = config.GetConnectionString(ConnectionStringNames.Sql);

            services.AddScoped(x =>
            {
                var builder = new DbContextOptionsBuilder<DbMigrations>();
                builder.UseSqlServer(conn, opt => opt.UseNetTopologySuite());

                return new DbMigrations(builder.Options);
            });
        }

        protected override Task RunAsync(IServiceProvider provider)
        {
            using (var context = provider.GetRequiredService<DbMigrations>())
            {
                context.Database.Migrate();
                context.EnsureSeedData();
            }

            return Task.CompletedTask;
        }
    }
}