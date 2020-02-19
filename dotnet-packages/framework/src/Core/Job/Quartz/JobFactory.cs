using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;

namespace Framework.Core.Job.Quartz
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _container;
        private readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new ConcurrentDictionary<IJob, IServiceScope>();

        public JobFactory(IServiceProvider container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _container.CreateScope();
            IJob job;

            try
            {
                job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }
            catch
            {
                scope.Dispose();
                throw;
            }

            if (job != null && _scopes.TryAdd(job, scope)) return job;

            scope.Dispose();

            return job;
        }

        public void AddJob(IJob job, IServiceScope service)
        {
            if (_scopes.TryAdd(job, service))
            {
                _scopes.AddOrUpdate(job, service, null);
            }
        }

        public void ReturnJob(IJob job)
        {
            if (_scopes.TryRemove(job, out var scope))
            {
                scope.Dispose();
            }
        }
    }
}