using Framework.Web.Common;
using System.Threading.Tasks;

namespace Library.Api
{
    public class Program
    {
        protected Program()
        {
        }

        public static async Task<int> Main()
        {
            return await WebHostBootstrap.RunAsync<Startup>();
        }
    }
}