using Framework.Core.Bus;
using MediatR;
using System;

namespace Bookstore.Domain.CommandSide.Commands
{
    public class PublishShippingEventCommand : BusMessage, IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
}