using Framework.Core.Bus;
using Framework.Core.Job.TaskRunner;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Tools.Tasks
{
    public class ReprocessDtoTaskRunner : BaseTask
    {
        protected override void Configure(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<DbLibrary>();
        }

        protected override async Task RunAsync(IServiceProvider provider)
        {
            List<Reservation> reservations = null;

            using (var context = provider.GetService<DbLibrary>())
            {
                reservations = await context.Reservations.ToListAsync();
            }

            foreach (var reservation in reservations)
            {
                var message = new PublishReservationEventCommand()
                {
                    ReservationId = reservation.Id
                };

                var bus = provider.GetService<IBusPublisher>();
                await bus.SendAsync(ContextNames.Queue.Library, message);
            }
        }
    }
}