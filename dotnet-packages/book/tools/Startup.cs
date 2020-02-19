using Book.Tools.Tasks;
using Framework.Core.Job.TaskRunner;
using BaseStartup = Framework.Core.Job.TaskRunner.BaseStartup;

namespace Book.Tools
{
    public class Startup : BaseStartup
    {
        protected override void RegisterTaskRunners(ITaskRunner runner)
        {
            var container = TaskContainerBuilder.Create()
                .Add<MigrationTaskRunner>()
                .Build();

            runner.SetContainer(container);
        }
    }
}