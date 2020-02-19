using Library.Infra.CrossCutting;
using Library.Job.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using BaseStartup = Framework.Core.Job.Quartz.BaseStartup;

namespace Library.Job
{
    public class Startup : BaseStartup
    {
        protected override void RegisterService(IServiceCollection services, IConfiguration config)
        {
            BootStrapper.RegisterServices(services, config);

            services.AddScoped<CheckReservationDueJob>();
        }

        protected override void RegisterJobs(IScheduler scheduler)
        {
            var job = JobBuilder.Create<CheckReservationDueJob>().Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(15)
                  .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}