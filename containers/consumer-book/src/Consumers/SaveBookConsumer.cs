using Book.Domain.CommandSide.Commands;
using Framework.Core.Bus.Consumer;
using MediatR;
using System.Threading.Tasks;

namespace Book.Consumer.Consumers
{
    public class SaveBookConsumer : BaseConsumer<SaveBookCommand>
    {
        private readonly IMediator _mediator;

        public SaveBookConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(SaveBookCommand message)
        {
            await _mediator.Send(message);
        }
    }
}