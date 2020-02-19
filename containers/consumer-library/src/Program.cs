using Framework.Core.Bus.Consumer;
using Library.Consumer;
using System.Threading.Tasks;

namespace Bookstore.Consumer
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