using System.Collections.Generic;

namespace Framework.Core.Job.TaskRunner
{
    public interface ITaskContainer
    {
        ITask GetById(int id);

        IEnumerable<(int Id, ITask Task)> GetAll();
    }
}