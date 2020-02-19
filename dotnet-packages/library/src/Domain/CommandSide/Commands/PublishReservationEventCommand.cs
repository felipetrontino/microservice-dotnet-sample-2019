using Framework.Core.Bus;
using MediatR;
using System;

namespace Library.Domain.CommandSide.Commands
{
    public class PublishReservationEventCommand : BusMessage, IRequest<bool>
    {
        public Guid ReservationId { get; set; }
    }
}