using Framework.Core.Bus;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.CommandSide.CommandHandlers
{
    public class ExpireReservationCommandHandler : IRequestHandler<ExpireReservationCommand, bool>
    {
        private readonly DbLibrary _db;
        private readonly IBusPublisher _bus;

        public ExpireReservationCommandHandler(DbLibrary db, IBusPublisher bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task<bool> Handle(ExpireReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _db.Reservations.FirstOrDefaultAsync(x => x.Id == request.ReservationId, cancellationToken);

            reservation.Status = StatusReservation.Expired;

            await _db.SaveChangesAsync(cancellationToken);

            var message = new PublishReservationEventCommand
            {
                ReservationId = reservation.Id
            };

            await _bus.SendAsync(ContextNames.Queue.Library, message);

            return true;
        }
    }
}