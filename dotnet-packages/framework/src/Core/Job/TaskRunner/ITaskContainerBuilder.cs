namespace Framework.Core.Job.TaskRunner
{
    public interface ITaskContainerBuilder
    {
        ITaskContainerBuilder Add<T>(string name = null) where T : ITask, new();

        ITaskContainer Build();
    }
}