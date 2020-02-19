using System.Collections.Generic;

namespace Framework.Core.Bus.Consumer
{
    internal class ConsumerContainer : IConsumerContainer
    {
        public ConsumerContainer(IEnumerable<ConsumerResolver> consumers)
        {
            Consumers = consumers;
        }

        protected IEnumerable<ConsumerResolver> Consumers { get; }

        public IEnumerable<ConsumerResolver> GetAll()
        {
            return Consumers;
        }
    }
}