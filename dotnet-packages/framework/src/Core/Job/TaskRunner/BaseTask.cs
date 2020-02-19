using Framework.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Framework.Core.Job.TaskRunner
{
    public abstract class BaseTask : ITask
    {
        protected BaseTask()
        {
            Name = GetType().Name;
        }

        public string Name { get; set; }

        protected virtual void Configure(IServiceCollection services, IConfiguration config)
        {
        }

        protected abstract Task RunAsync(IServiceProvider provider);

        public async Task Execute(IServiceCollection services, IConfiguration config)
        {
            LogHelper.Debug($"Starting {Name}...");

            try
            {
                using var serviceScope = GetServiceScope(services, config);

                var watch = Stopwatch.StartNew();

                await RunAsync(serviceScope.ServiceProvider);

                watch.Stop();

                LogHelper.Debug($"Exiting { Name }: { watch.Elapsed }...");
            }
            catch (Exception e)
            {
                LogHelper.Error($"Error {Name}: {e.Message}", e);
            }
        }

        private IServiceScope GetServiceScope(IServiceCollection services, IConfiguration config)
        {
            Configure(services, config);

            var provider = services.BuildServiceProvider();
            return provider.CreateScope();
        }
    }
}