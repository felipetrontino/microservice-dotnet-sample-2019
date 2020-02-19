using Bookstore.Consumer.Consumers;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Common;
using Bookstore.Infra.CrossCutting;
using Framework.Core.Bus.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Consumer
{
    public class Startup : BaseStartup
    {
        protected override void RegisterService(IServiceCollection services, IConfiguration config)
        {
            BootStrapper.RegisterServices(services, config);

            services.AddScoped<IConsumer<PurchaseBookCommand>, PurchaseBookConsumer>();
            services.AddScoped<IConsumer<PublishShippingEventCommand>, PublishShippingEventConsumer>();
            services.AddScoped<IConsumer<UpdateBookCommand>, UpdateBookConsumer>();
        }

        protected override void RegisterConsumers(IConsumerHandler handler)
        {
            var container = ConsumerContainerBuilder.Create()
                .Queue<UpdateBookCommand>(ContextNames.Content.Purchase, ContextNames.Queue.Bookstore, ContextNames.Exchange.Bookstore)
                .Queue<UpdateBookCommand>(ContextNames.Content.ShippingEvent, ContextNames.Queue.Bookstore, ContextNames.Exchange.Bookstore)
                .Queue<UpdateBookCommand>(ContextNames.Content.UpdateBook, ContextNames.Queue.Book_Bookstore, ContextNames.Exchange.Bookstore)
                .Build();

            handler.SetContainer(container);
        }
    }
}