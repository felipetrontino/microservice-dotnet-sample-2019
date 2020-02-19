using Framework.Core.Bus;

namespace Bookstore.Domain.Models.Event
{
    public class DropCopyNumberEvent : BaseMessage
    {
        public string Number { get; set; }
    }
}