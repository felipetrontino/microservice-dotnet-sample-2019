using RabbitMQ.Client;

namespace Framework.Core.Bus.RabbitMQ
{
    public interface IBusConnection
    {
        IConnection GetConnection();
    }
}