using System.Threading.Tasks;

namespace Framework.Core.Bus.Consumer
{
    public interface IConsumer<in TMessage>
         where TMessage : IBusMessage
    {
        Task Execute(TMessage message);
    }
}