using Framework.Core.Bus;
using Framework.Core.Common;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.CommandSide.CommandHandlers
{
    public class CheckDueCommandHandler : IRequestHandler<CheckDueCommand, bool>
    {
        private readonly DbLibrary _db;
        private readonly IBusPublisher _bus;
        private readonly IDateTimeService _dateTimeService;

        public CheckDueCommandHandler(DbLibrary db, IBusPublisher bus, IDateTimeService dateTimeService)
        {
            _db = db;
            _bus = bus;
            _dateTimeService = dateTimeService;
        }

        public async Task<bool> Handle(CheckDueCommand request, CancellationToken cancellationToken)
        {
            var date = _dateTimeService.Now().Date.AddDays(1).AddSeconds(-1);

            var expiredList = await _db.Reservations
                                      .Where(x => x.Loans.Any(l => l.DueDate.Date <= date))
                                      .ToListAsync(cancellationToken);

            foreach (var item in expiredList)
            {
                var message = new ExpireReservationCommand
                {
                    ReservationId = item.Id
                };

                await _bus.SendAsync(ContextNames.Queue.Library, message);
            }

            return true;
        }
    }
}