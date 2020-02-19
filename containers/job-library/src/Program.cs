using Framework.Core.Job.Quartz;
using System.Threading.Tasks;

namespace Library.Job
{
    public class Program
    {
        protected Program()
        {
        }

        public static async Task<int> Main()
        {
            return await QuartzJobBootstrap.RunAsync<Startup>();
        }
    }
}