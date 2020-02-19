using Book.Domain.Enums;
using Framework.Core.Bus;

namespace Book.Models.Event
{
    public class UpdateBookEvent : BusMessage
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public Language Language { get; set; }
    }
}