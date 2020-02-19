using System.Collections.Generic;

namespace Framework.Core.Bus.Consumer
{
    public interface IConsumerContainer
    {
        IEnumerable<ConsumerResolver> GetAll();
    }
}