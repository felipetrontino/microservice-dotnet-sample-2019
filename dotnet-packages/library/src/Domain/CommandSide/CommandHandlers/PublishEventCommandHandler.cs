using Framework.Core.Bus;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Domain.Models.Event;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.CommandSide.CommandHandlers
{
    public class PublishEventCommandHandler : IRequestHandler<PublishReservationEventCommand, bool>
    {
        private readonly DbLibrary _db;
        private readonly IBusPublisher _bus;

        public PublishEventCommandHandler(DbLibrary db, IBusPublisher bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task<bool> Handle(PublishReservationEventCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _db.Reservations
                                       .Include(x => x.Loans)
                                         .ThenInclude(x => x.Copy)
                                            .ThenInclude(x => x.Book)
                                       .Include(x => x.Member)
                                       .FirstOrDefaultAsync(x => x.Id == request.ReservationId, cancellationToken);

            var output = new CreateReservationEvent
            {
                Number = reservation.Number,
                RequestDate = reservation.RequestDate,
                Status = reservation.Status,

                Member = new CreateReservationEvent.MemberDetail()
                {
                    DocumentId = reservation.Member.DocumentId,
                    Name = reservation.Member.Name
                },

                Loans = new List<CreateReservationEvent.LoanDetail>()
            };

            foreach (var item in reservation.Loans)
            {
                var loanDto = new CreateReservationEvent.LoanDetail()
                {
                    CopyNumber = item.Copy.Number,
                    Title = item.Copy.Book.Title,
                    DueDate = item.DueDate,
                    ReturnDate = item.ReturnDate
                };

                output.Loans.Add(loanDto);
            }

            await _bus.PublishAsync(ContextNames.Exchange.Library, output);

            return true;
        }
    }
}