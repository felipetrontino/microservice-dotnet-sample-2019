using Framework.Core.Bus;
using Library.Domain.Enums;
using MediatR;

namespace Library.Domain.CommandSide.Commands
{
    public class UpdateBookCommand : BusMessage, IRequest<bool>
    {
        public string Title { get; set; }
        public Language Language { get; set; }

        public string Author { get; set; }
    }
}