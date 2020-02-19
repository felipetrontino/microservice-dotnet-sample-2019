using Book.Consumer.Consumers;
using Book.Domain.CommandSide.Commands;
using Book.Domain.Common;
using Book.Infra.CrossCutting;
using Framework.Core.Bus.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Book.Consumer
{
    public class Startup : BaseStartup
    {
        protected override void RegisterService(IServiceCollection services, IConfiguration config)
        {
            BootStrapper.RegisterServices(services, config);
            services.AddScoped<IConsumer<SaveBookCommand>, SaveBookConsumer>();
        }

        protected override void RegisterConsumers(IConsumerHandler handler)
        {
            var container = ConsumerContainerBuilder.Create()
                .Queue<SaveBookCommand>(ContextNames.Content.Book, ContextNames.Queue.Book, ContextNames.Exchange.Book)
                .Build();

            handler.SetContainer(container);
        }
    }
}