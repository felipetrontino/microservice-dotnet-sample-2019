using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Test.Mock.Bus
{
    public class BusTaskContainer
    {
        private List<(string Name, Func<Task> Task)> _tasks = new List<(string Name, Func<Task> Task)>();

        public static BusTaskContainer Create() => new BusTaskContainer();

        public void Add(string name, Func<Task> task)
        {
            _tasks.Add((name, task));
        }

        public void WhenAll(Func<string, Task> innerTask = null)
        {
            var newTasks = innerTask == null ? _tasks.Select(x => x.Task()).ToList() : new List<Task>();

            if (innerTask != null)
            {
                foreach (var (name, task) in _tasks)
                {
                    var newTask = Task.Run(async () =>
                    {
                        await innerTask(name);
                        await task();
                    });

                    newTasks.Add(newTask);
                }
            }

            Task.WhenAll(newTasks).Wait();
            _tasks = new List<(string Name, Func<Task> Task)>();
        }
    }
}