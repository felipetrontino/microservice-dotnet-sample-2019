using Bookstore.Domain.Enums;
using Framework.Core.Bus;
using MediatR;

namespace Bookstore.Domain.CommandSide.Commands
{
    public class UpdateBookCommand : BusMessage, IRequest<bool>
    {
        public string Title { get; set; }
        public Language Language { get; set; }
        public string Author { get; set; }
    }
}