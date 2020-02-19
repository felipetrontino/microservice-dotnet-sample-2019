using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Framework.Core.Job.TaskRunner
{
    public interface ITaskRunner
    {
        void SetContainer(ITaskContainer container);

        Task RunAsync(string[] args, IServiceCollection services, IConfiguration config);
    }
}