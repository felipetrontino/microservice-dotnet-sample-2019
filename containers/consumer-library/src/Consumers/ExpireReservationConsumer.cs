using Framework.Core.Bus.Consumer;
using Library.Domain.CommandSide.Commands;
using MediatR;
using System.Threading.Tasks;

namespace Library.Consumer.Consumers
{
    public class ExpireReservationConsumer : BaseConsumer<ExpireReservationCommand>
    {
        private readonly IMediator _mediator;

        public ExpireReservationConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(ExpireReservationCommand message)
        {
            await _mediator.Send(message);
        }
    }
}