using Framework.Core.Bus.Consumer;
using Library.Domain.CommandSide.Commands;
using MediatR;
using System.Threading.Tasks;

namespace Library.Consumer.Consumers
{
    public class ReserveBookConsumer : BaseConsumer<ReserveBookCommand>
    {
        private readonly IMediator _mediator;

        public ReserveBookConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(ReserveBookCommand message)
        {
            await _mediator.Send(message);
        }
    }
}