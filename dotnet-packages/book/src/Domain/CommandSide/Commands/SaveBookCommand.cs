using Book.Domain.Enums;
using Framework.Core.Bus;
using MediatR;

namespace Book.Domain.CommandSide.Commands
{
    public class SaveBookCommand : BusMessage, IRequest<bool>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public Language Language { get; set; }
    }
}