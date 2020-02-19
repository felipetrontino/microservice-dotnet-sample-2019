using Framework.Core.Job.TaskRunner;
using Library.Tools.Tasks;
using BaseStartup = Framework.Core.Job.TaskRunner.BaseStartup;

namespace Library.Tools
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