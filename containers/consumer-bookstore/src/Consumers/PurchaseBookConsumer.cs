using Bookstore.Domain.CommandSide.Commands;
using Framework.Core.Bus.Consumer;
using MediatR;
using System.Threading.Tasks;

namespace Bookstore.Consumer.Consumers
{
    public class PurchaseBookConsumer : BaseConsumer<PurchaseBookCommand>
    {
        private readonly IMediator _mediator;

        public PurchaseBookConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task Execute(PurchaseBookCommand message)
        {
            await _mediator.Send(message);
        }
    }
}