using Bookstore.Domain.CommandSide.Commands;
using Framework.Core.Bus.Consumer;
using MediatR;
using System.Threading.Tasks;

namespace Bookstore.Consumer.Consumers
{
    public class UpdateBookConsumer : BaseConsumer<UpdateBookCommand>
    {
        private readonly IMediator _mediator;

        public UpdateBookConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(UpdateBookCommand message)
        {
            await _mediator.Send(message);
        }
    }
}