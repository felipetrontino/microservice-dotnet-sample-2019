using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Job.TaskRunner
{
    public class TaskContainer : ITaskContainer
    {
        private readonly List<ITask> _tasks;

        public TaskContainer(List<ITask> tasks)
        {
            _tasks = tasks;
        }

        public IEnumerable<(int Id, ITask Task)> GetAll()
        {
            return _tasks.Select((x, i) => (i + 1, x)).ToList();
        }

        public ITask GetById(int id)
        {
            return id > 0 && _tasks.Count >= id ? _tasks[id - 1] : null;
        }
    }
}