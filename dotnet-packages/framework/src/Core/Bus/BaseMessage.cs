using System;

namespace Framework.Core.Bus
{
    public abstract class BaseMessage : IBusMessage
    {
        protected BaseMessage()
        {
            MessageId = Guid.NewGuid().ToString();
            ContentName = GetType().Name;
        }

        public virtual string ContentName { get; }
        public string MessageId { get; }
    }
}