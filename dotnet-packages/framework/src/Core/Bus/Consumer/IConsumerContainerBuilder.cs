namespace Framework.Core.Bus.Consumer
{
    public interface IConsumerContainerBuilder
    {
        IConsumerContainerBuilder Queue<TMessage>(string contentName, string queueName, string exchangeName = null, string topic = null) where TMessage : IBusMessage;

        IConsumerContainer Build();
    }
}