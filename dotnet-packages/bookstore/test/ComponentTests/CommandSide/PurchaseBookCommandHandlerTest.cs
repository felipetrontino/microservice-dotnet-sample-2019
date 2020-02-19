using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Common;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Models.Event;
using Bookstore.Infra.CrossCutting;
using Bookstore.Tests.Mocks.Commands;
using Bookstore.Tests.Mocks.Entities;
using Bookstore.Tests.Mocks.Models.Event;
using FluentAssertions;
using Framework.Core.Bus;
using Framework.Core.Common;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using Framework.Test.Mock.Bus;
using Framework.Test.Mock.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using Xunit;

namespace Bookstore.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class PurchaseBookCommandHandlerTest
    {
        protected readonly IMockRepository<DbBookstore> MockRepository;
        protected readonly BusPublisherStub Bus;

        public PurchaseBookCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbBookstore>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_PurchaseBookCommand_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var command = PurchaseBookCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var orders = MockRepository.Query<Order>()
                                       .Include(x => x.Items)
                                       .Include(x => x.Customer)
                                       .ToList();
            var orderExpected = OrderMock.Get(key);
            orders.Should().BeEquivalentToEntity(MockBuilder.List(orderExpected));

            var publishShippingEventCommands = Bus.GetAllSent<PublishShippingEventCommand>(ContextNames.Queue.Bookstore);
            var publishShippingEventCommandExpected = PublishShippingEventCommandMock.Get(key);
            publishShippingEventCommandExpected.OrderId = orders[0].Id;
            publishShippingEventCommands.Should().BeEquivalentToMessage(MockBuilder.List(publishShippingEventCommandExpected));

            var dropCopyNumberEvents = Bus.GetAllPublished<DropCopyNumberEvent>(ContextNames.Exchange.Bookstore);
            var dropCopyNumberEventExpected = DropCopyNumberEventMock.Get(key);
            dropCopyNumberEvents.Should().BeEquivalentToMessage(MockBuilder.List(dropCopyNumberEventExpected));
        }

        [Fact]
        public void Handle_PurchaseBookCommand_Update()
        {
            // arrange
            var key = MockBuilder.Key;

            var order = OrderMock.Get(key);
            MockRepository.Add(order);

            MockRepository.Commit();

            var key2 = MockBuilder.Key;
            var command = PurchaseBookCommandMock.Get(key2);
            command.Number = order.Number;

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var orders = MockRepository.Query<Order>()
                                      .Include(x => x.Items)
                                      .Include(x => x.Customer)
                                      .ToList();
            var orderExpected = OrderMock.Create(key)
                                        .BaseOrder()
                                        .WithItem(key2)
                                        .Build();
            orderExpected.Number = order.Number;
            orders.Should().BeEquivalentToEntity(MockBuilder.List(orderExpected));

            var publishShippingEventCommands = Bus.GetAllSent<PublishShippingEventCommand>(ContextNames.Queue.Bookstore);
            var publishShippingEventCommandExpected = PublishShippingEventCommandMock.Get(key);
            publishShippingEventCommandExpected.OrderId = orders[0].Id;
            publishShippingEventCommands.Should().BeEquivalentToMessage(MockBuilder.List(publishShippingEventCommandExpected));

            var dropCopyNumberEvents = Bus.GetAllPublished<DropCopyNumberEvent>(ContextNames.Exchange.Bookstore);
            var dropCopyNumberEventExpected = DropCopyNumberEventMock.Get(key2);
            dropCopyNumberEvents.Should().BeEquivalentToMessage(MockBuilder.List(dropCopyNumberEventExpected));
        }

        [Fact]
        public void Handle_PurchaseBookCommand_Customer_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var customer = CustomerMock.GetFull(key);
            MockRepository.Add(customer);

            MockRepository.Commit();

            var command = PurchaseBookCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var customers = MockRepository.Query<Customer>().ToList();
            customers.Should().BeEquivalentToEntity(MockBuilder.List(customer));

            var orders = MockRepository.Query<Order>()
                                      .Include(x => x.Items)
                                      .Include(x => x.Customer)
                                      .ToList();
            var orderExpected = OrderMock.Get(key);
            orderExpected.Customer = customer;
            orders.Should().BeEquivalentToEntity(MockBuilder.List(orderExpected));
        }

        [Fact]
        public void Handle_PurchaseBookCommand_Customer_Not_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var command = PurchaseBookCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var customers = MockRepository.Query<Customer>().ToList();
            var customerExpected = CustomerMock.Get(key);
            customers.Should().BeEquivalentToEntity(MockBuilder.List(customerExpected));

            var orders = MockRepository.Query<Order>()
                                      .Include(x => x.Items)
                                      .Include(x => x.Customer)
                                      .ToList();
            var orderExpected = OrderMock.Get(key);
            orderExpected.Customer = customerExpected;
            orders.Should().BeEquivalentToEntity(MockBuilder.List(orderExpected));
        }

        private bool Handle(PurchaseBookCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
                s.AddScoped<IDateTimeService>(x => DateTimeServiceStub.Create());
            });

            var handler = provider.GetRequiredService<IRequestHandler<PurchaseBookCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}