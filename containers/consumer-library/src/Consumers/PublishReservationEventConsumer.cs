using Framework.Core.Bus.Consumer;
using Library.Domain.CommandSide.Commands;
using MediatR;
using System.Threading.Tasks;

namespace Library.Consumer.Consumers
{
    public class PublishReservationEventConsumer : BaseConsumer<PublishReservationEventCommand>
    {
        private readonly IMediator _mediator;

        public PublishReservationEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(PublishReservationEventCommand message)
        {
            await _mediator.Send(message);
        }
    }
}