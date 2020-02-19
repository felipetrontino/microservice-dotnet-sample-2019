using Framework.Core.Common;
using Framework.Core.Config;
using Framework.Core.Logging;
using Framework.Core.Logging.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using Framework.Core.Helpers;

namespace Framework.Core.Job.Quartz
{
    public static class QuartzJobBootstrap
    {
        public static async Task<int> RunAsync<TStartup>()
            where TStartup : class
        {
            var type = typeof(TStartup);
            var startup = Activator.CreateInstance<TStartup>();

            var ret = await RunAsync((s, c) => type.GetMethod(nameof(BaseStartup.ConfigureServices))?.Invoke(startup, new object[] { s, c }),
                (s) => type.GetMethod(nameof(BaseStartup.AddJobs))?.Invoke(startup, new object[] { s }));

            return ret;
        }

        private static async Task<int> RunAsync(Action<IServiceCollection, IConfiguration> configure, Action<IScheduler> addJobs)
        {
            var config = Configuration.GetConfiguration();
            LogHelper.Logger = new SerilogLogger();

            try
            {
                LogHelper.Info("Starting QuartzJob...");

                var services = new ServiceCollection();
                Bootstrapper.RegisterServices(services, config);

                configure(services, config);

                var factory = new StdSchedulerFactory();
                var scheduler = await factory.GetScheduler();
                scheduler.JobFactory = new JobFactory(services.BuildServiceProvider());

                configure(services, config);

                addJobs(scheduler);

                await scheduler.Start();

                LogHelper.Info($"Exiting QuartzJob...");

                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("QuartzJob terminated unexpectedly.", ex);
                return 1;
            }
            finally
            {
                LogHelper.Info($"While...");
                PromptHelper.Wait();
            }
        }
    }
}