using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using Framework.Core.Bus;
using Framework.Core.Job.TaskRunner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookstore.Tools.Tasks
{
    public class ReprocessDtoTaskRunner : BaseTask
    {
        protected override void Configure(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<DbBookstore>();
        }

        protected override async Task RunAsync(IServiceProvider provider)
        {
            List<Order> orders = null;

            using (var context = provider.GetService<DbBookstore>())
            {
                orders = await context.Orders.ToListAsync();
            }

            foreach (var order in orders)
            {
                var message = new PublishShippingEventCommand()
                {
                    OrderId = order.Id
                };

                var bus = provider.GetService<IBusPublisher>();
                await bus.SendAsync(ContextNames.Queue.Bookstore, message);
            }
        }
    }
}