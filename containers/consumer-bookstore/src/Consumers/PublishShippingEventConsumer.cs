using Bookstore.Domain.CommandSide.Commands;
using Framework.Core.Bus.Consumer;
using MediatR;
using System.Threading.Tasks;

namespace Bookstore.Consumer.Consumers
{
    public class PublishShippingEventConsumer : BaseConsumer<PublishShippingEventCommand>
    {
        private readonly IMediator _mediator;

        public PublishShippingEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(PublishShippingEventCommand message)
        {
            await _mediator.Send(message);
        }
    }
}