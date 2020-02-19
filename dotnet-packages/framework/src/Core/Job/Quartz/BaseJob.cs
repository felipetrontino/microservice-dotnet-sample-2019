using Framework.Core.Logging;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Framework.Core.Job.Quartz
{
    [DisallowConcurrentExecution]
    public abstract class BaseJob : IJob
    {
        protected BaseJob()
        {
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var name = context.JobDetail.Key.Name;

            try
            {
                LogHelper.Debug($"Starting {name}...");

                var watch = Stopwatch.StartNew();

                await Process();

                watch.Stop();

                LogHelper.Debug($"Exiting { name }: { watch.Elapsed }...");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{name} terminated unexpectedly.", ex);
            }
        }

        protected abstract Task Process();
    }
}