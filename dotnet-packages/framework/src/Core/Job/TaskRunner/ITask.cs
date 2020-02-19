using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Framework.Core.Job.TaskRunner
{
    public interface ITask
    {
        string Name { get; set; }

        Task Execute(IServiceCollection services, IConfiguration config);
    }
}