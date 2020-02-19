using System.Collections.Generic;

namespace Framework.Core.Job.TaskRunner
{
    public class TaskContainerBuilder : ITaskContainerBuilder
    {
        private readonly List<ITask> _tasks = new List<ITask>();

        public static ITaskContainerBuilder Create() => new TaskContainerBuilder();

        public ITaskContainerBuilder Add<T>(string name = null)
            where T : ITask, new()
        {
            var task = new T();

            if (name != null)
                task.Name = name;

            _tasks.Add(task);

            return this;
        }

        public ITaskContainer Build()
        {
            return new TaskContainer(_tasks);
        }
    }
}