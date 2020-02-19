using Bookstore.Tools.Tasks;
using Framework.Core.Job.TaskRunner;
using BaseStartup = Framework.Core.Job.TaskRunner.BaseStartup;

namespace Bookstore.Tools
{
    public class Startup : BaseStartup
    {
        protected override void RegisterTaskRunners(ITaskRunner runner)
        {
            var container = TaskContainerBuilder.Create()
                .Add<MigrationTaskRunner>()
                .Add<ReprocessDtoTaskRunner>()
                .Build();

            runner.SetContainer(container);
        }
    }
}