using Framework.Core.Common;
using Framework.Core.Config;
using Framework.Core.Logging;
using Framework.Core.Logging.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Framework.Core.Job.TaskRunner
{
    public static class TaskRunnerBootstrap
    {
        public static async Task<int> RunAsync<TStartup>(string[] args)
            where TStartup : class
        {
            var type = typeof(TStartup);
            var startup = Activator.CreateInstance<TStartup>();

            var ret = await RunAsync(args, (s, c) => type.GetMethod(nameof(BaseStartup.ConfigureServices))?.Invoke(startup, new object[] { s, c }),
                (r) => type.GetMethod(nameof(BaseStartup.AddTaskRunners))?.Invoke(startup, new object[] { r }));

            return ret;
        }

        private static async Task<int> RunAsync(string[] args, Action<IServiceCollection, IConfiguration> configure, Action<ITaskRunner> addTaskRunners)
        {
            var config = Configuration.GetConfiguration();
            LogHelper.Logger = new SerilogLogger();

            try
            {
                LogHelper.Info("Starting TaskRunner...");

                var services = new ServiceCollection();
                Bootstrapper.RegisterServices(services, config);

                configure(services, config);

                var runner = ConsoleRunner.Create();

                addTaskRunners(runner);

                await runner.RunAsync(args, services, config);

                LogHelper.Info($"Exiting TaskRunner...");

                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("TaskRunner terminated unexpectedly.", ex);
                return 1;
            }
            finally
            {
#if DEBUG
                Console.ReadKey();
#endif
            }
        }
    }
}