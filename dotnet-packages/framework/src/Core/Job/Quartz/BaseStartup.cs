using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Framework.Core.Job.Quartz
{
    public abstract class BaseStartup
    {
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            RegisterService(services, config);
        }

        protected virtual void RegisterService(IServiceCollection services, IConfiguration config)
        {
        }

        public void AddJobs(IScheduler runner)
        {
            RegisterJobs(runner);
        }

        protected virtual void RegisterJobs(IScheduler scheduler)
        {
        }
    }
}