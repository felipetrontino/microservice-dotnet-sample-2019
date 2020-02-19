using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Job.TaskRunner
{
    public abstract class BaseStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            RegisterService(services, config);
        }

        protected virtual void RegisterService(IServiceCollection services, IConfiguration config)
        {
        }

        public void AddTaskRunners(ITaskRunner runner)
        {
            RegisterTaskRunners(runner);
        }

        protected virtual void RegisterTaskRunners(ITaskRunner runner)
        {
        }
    }
}