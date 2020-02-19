using System.Threading.Tasks;

namespace Framework.Core.Bus.Consumer
{
    public interface IConsumerHandler
    {
        Task StartAsync();

        void SetContainer(IConsumerContainer container);
    }
}