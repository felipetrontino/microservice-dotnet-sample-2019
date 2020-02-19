using Framework.Core.Bus.Consumer;
using System.Threading.Tasks;

namespace Book.Consumer
{
    public class Program
    {
        protected Program()
        {
        }

        public static async Task<int> Main()
        {
            return await ConsumerBootstrap.RunAsync<Startup>();
        }
    }
}