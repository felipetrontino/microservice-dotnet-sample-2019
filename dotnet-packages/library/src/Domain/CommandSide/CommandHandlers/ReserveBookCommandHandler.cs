using Framework.Core.Bus;
using Framework.Core.Common;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.CommandSide.CommandHandlers
{
    public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand, bool>
    {
        private readonly DbLibrary _db;
        private readonly IBusPublisher _bus;
        private readonly IDateTimeService _dateTimeService;

        public ReserveBookCommandHandler(DbLibrary db, IBusPublisher bus, IDateTimeService dateTimeService)
        {
            _db = db;
            _bus = bus;
            _dateTimeService = dateTimeService;
        }

        public async Task<bool> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _db.Reservations
                .Include(x => x.Loans)
                .Include(x => x.Member)
                .FirstOrDefaultAsync(x => x.Number == request.Number, cancellationToken);

            if (reservation == null)
            {
                reservation = new Reservation
                {
                    RequestDate = _dateTimeService.Now(),
                    Status = StatusReservation.Opened,
                    Number = request.Number,
                    Member = await _db.Members.FirstOrDefaultAsync(x => x.DocumentId == request.MemberId, cancellationToken)
                };

                if (reservation.Member == null)
                {
                    reservation.Member = new Member
                    {
                        DocumentId = request.MemberId,
                        Name = request.MemberName
                    };
                }

                await _db.AddAsync(reservation, cancellationToken);
            }

            reservation.Loans = new List<Loan>();

            foreach (var item in request.Items)
            {
                var loan = new Loan
                {
                    Copy = await _db.Copies.FirstOrDefaultAsync(x => x.Number == item.Number, cancellationToken),
                    DueDate = reservation.RequestDate.AddDays(7)
                };

                reservation.Loans.Add(loan);
            }

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