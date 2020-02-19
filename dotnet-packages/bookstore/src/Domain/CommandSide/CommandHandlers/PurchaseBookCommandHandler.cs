using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Enums;
using Bookstore.Domain.Models.Event;
using Framework.Core.Bus;
using Framework.Core.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bookstore.Domain.CommandSide.CommandHandlers
{
    public class PurchaseBookCommandHandler : IRequestHandler<PurchaseBookCommand, bool>
    {
        private readonly DbBookstore _db;
        private readonly IBusContainer _bus;
        private readonly IDateTimeService _dateTimeService;

        public PurchaseBookCommandHandler(DbBookstore db, IBusContainer bus, IDateTimeService dateTimeService)
        {
            _db = db;
            _bus = bus;
            _dateTimeService = dateTimeService;
        }

        public async Task<bool> Handle(PurchaseBookCommand request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders
                .Include(x => x.Items)
                    .ThenInclude(x => x.Book)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Number == request.Number, cancellationToken);

            if (order == null)
            {
                order = new Order
                {
                    Number = request.Number,
                    Status = StatusOrder.Opened,
                    CreateDate = _dateTimeService.Now(),
                    Customer = await _db.Customers.FirstOrDefaultAsync(x => x.DocumentId == request.CustomerId, cancellationToken)
                };

                if (order.Customer == null)
                {
                    order.Customer = new Customer
                    {
                        DocumentId = request.CustomerId,
                        Name = request.CustomerName
                    };
                }

                await _db.AddAsync(order, cancellationToken);
            }

            order.Items = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var orderItem = await GetItemAsync(_db, item, cancellationToken);

                var dropCopyNumberEvent = new DropCopyNumberEvent() { Number = item.Number };
                await _bus.PublishAsync(ContextNames.Queue.Bookstore, dropCopyNumberEvent);

                order.Items.Add(orderItem);
            }

            await _db.SaveChangesAsync(cancellationToken);

            var dto = new PublishShippingEventCommand
            {
                OrderId = order.Id
            };

            await _bus.SendAsync(ContextNames.Queue.Bookstore, dto);

            await _bus.CommitAsync();

            return true;
        }

        private async Task<OrderItem> GetItemAsync(DbBookstore db, PurchaseBookCommand.Item item, CancellationToken cancellationToken)
        {
            var ret = new OrderItem
            {
                Book = await db.Books.FirstOrDefaultAsync(x => x.Title.Contains(item.Name), cancellationToken),
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity
            };

            ret.Total = ret.Price * ret.Quantity;

            return ret;
        }
    }
}