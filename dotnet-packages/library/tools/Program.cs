using Framework.Core.Job.TaskRunner;
using System.Threading.Tasks;

namespace Library.Tools
{
    public class Program
    {
        protected Program()
        {
        }

        public static async Task<int> Main(string[] args)
        {
            return await TaskRunnerBootstrap.RunAsync<Startup>(args);
        }
    }
}