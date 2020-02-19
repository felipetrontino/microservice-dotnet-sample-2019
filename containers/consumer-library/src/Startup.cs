using Framework.Core.Bus.Consumer;
using Library.Consumer.Consumers;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Infra.CrossCutting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Consumer
{
    public class Startup : BaseStartup
    {
        protected override void RegisterService(IServiceCollection services, IConfiguration config)
        {
            BootStrapper.RegisterServices(services, config);

            services.AddScoped<IConsumer<ReserveBookCommand>, ReserveBookConsumer>();
            services.AddScoped<IConsumer<PublishReservationEventCommand>, PublishReservationEventConsumer>();
            services.AddScoped<IConsumer<ExpireReservationCommand>, ExpireReservationConsumer>();
            services.AddScoped<IConsumer<UpdateBookCommand>, UpdateBookConsumer>();
        }

        protected override void RegisterConsumers(IConsumerHandler handler)
        {
            var container = ConsumerContainerBuilder.Create()
                .Queue<UpdateBookCommand>(ContextNames.Content.Reservation, ContextNames.Queue.Library, ContextNames.Exchange.Library)
                .Queue<UpdateBookCommand>(ContextNames.Content.ReservationEvent, ContextNames.Queue.Library, ContextNames.Exchange.Library)
                .Queue<UpdateBookCommand>(ContextNames.Content.ReservationExpired, ContextNames.Queue.Library, ContextNames.Exchange.Library)
                .Queue<UpdateBookCommand>(ContextNames.Content.UpdateBook, ContextNames.Queue.Book_Library, ContextNames.Exchange.Book)
                .Build();

            handler.SetContainer(container);
        }
    }
}