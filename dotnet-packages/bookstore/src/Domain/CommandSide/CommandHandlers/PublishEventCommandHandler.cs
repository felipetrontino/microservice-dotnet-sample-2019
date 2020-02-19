using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models.Event;
using Framework.Core.Bus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Domain.CommandSide.CommandHandlers
{
    public class PublishEventCommandHandler : IRequestHandler<PublishShippingEventCommand, bool>
    {
        private readonly DbBookstore _db;
        private readonly IBusPublisher _bus;

        public PublishEventCommandHandler(DbBookstore db, IBusPublisher bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task<bool> Handle(PublishShippingEventCommand request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders
              .Include(x => x.Items)
              .Include(x => x.Customer)
              .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            var output = new CreateShippingEvent
            {
                Number = order.Number,
                Status = order.Status,
                CreateDate = order.CreateDate,
                Customer = new CreateShippingEvent.CustomerDetail()
                {
                    Name = order.Customer.Name,
                    Address = order.Customer.Address
                }
            };

            foreach (var item in order.Items)
            {
                var itemDto = new CreateShippingEvent.OrderItemDetail()
                {
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Total = item.Total
                };

                output.Items.Add(itemDto);
            }

            await _bus.PublishAsync(ContextNames.Exchange.Bookstore, output);

            return true;
        }
    }
}