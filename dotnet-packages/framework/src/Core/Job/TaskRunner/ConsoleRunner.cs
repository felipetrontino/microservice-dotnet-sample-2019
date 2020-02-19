using Framework.Core.Config;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace Framework.Core.Job.TaskRunner
{
    public class ConsoleRunner : ITaskRunner
    {
        public static ITaskRunner Create() => new ConsoleRunner();

        protected ITaskContainer Container { get; private set; }

        public void SetContainer(ITaskContainer container)
        {
            Container = container;
        }

        public async Task RunAsync(string[] args, IServiceCollection services, IConfiguration config)
        {
            while (true)
            {
                Clear();

                if (Container == null)
                    throw new InvalidOperationException();

                WriteLine($"### {Configuration.AppName.Get()} ###");

                var isAutomatic = args.Length > 0;
                var value = isAutomatic ? args[0] : string.Empty;
                var taskRunner = ChooseTask(isAutomatic, value);

                if (taskRunner != null)
                    await taskRunner.Execute(services, config);
                else
                    WriteLine("Option not found.");

                if (isAutomatic) return;

                WriteLine("Done. Press a key.");
                ReadKey();
            }
        }

        private ITask ChooseTask(bool isAutomatic, string value)
        {
            var option = 0;
            var containers = Container.GetAll().ToList();

            if (isAutomatic)
            {
                int.TryParse(value, out option);

                if (option == 0)
                {
                    var (id, task) = containers.FirstOrDefault(x => x.Task.Name == value);
                    option = task != null ? id : -1;
                }
            }
            else
            {
                option = Prompt.GetInt(string.Join(Environment.NewLine, containers.Select((x, i) => x.Id + ". " + x.Task.Name).Append("0. Exit" + Environment.NewLine)));
            }

            return option == 0 ? null : Container.GetById(option);
        }
    }
}