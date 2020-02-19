using FluentAssertions;
using Framework.Core.Bus;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Bus
{
    public class BusExtensionsTest
    {
        #region PublishAsync

        [Fact]
        public void PublishAsync_Valid()
        {
            // arrange
            var published = false;
            var exchangeName = GetExchangeName();
            var message = GetMessage();

            var bus = Substitute.For<IBusPublisher>();
            bus.PublishAsync(Arg.Any<string>(), Arg.Any<IEnumerable<Message>>())
                .Returns(Task.CompletedTask)
                .AndDoes(x => published = true);

            // act
            PublishAsync(bus, exchangeName, message);

            // assert
            published.Should().BeTrue();
        }

        #endregion PublishAsync

        #region PublishAsync [Topic]

        [Fact]
        public void PublishAsync_Topic_Valid()
        {
            // arrange
            var published = false;
            var exchangeName = GetExchangeName();
            var topic = GetTopic();
            var message = GetMessage();

            var bus = Substitute.For<IBusPublisher>();
            bus.PublishAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<Message>>())
                .Returns(Task.CompletedTask)
                .AndDoes(x => published = true);

            // act
            PublishAsync(bus, exchangeName, topic, message);

            // assert
            published.Should().BeTrue();
        }

        #endregion PublishAsync [Topic]

        #region SendAsync

        [Fact]
        public void SendAsync_Valid()
        {
            // arrange
            var published = false;
            var queueName = GetQueueName();
            var message = GetMessage();

            var bus = Substitute.For<IBusPublisher>();
            bus.SendAsync(Arg.Any<string>(), Arg.Any<IEnumerable<Message>>())
                .Returns(Task.CompletedTask)
                .AndDoes(x => published = true);

            // act
            SendAsync(bus, queueName, message);

            // assert
            published.Should().BeTrue();
        }

        #endregion SendAsync

        private static void PublishAsync<TMessage>(IBusPublisher publisher, string exchangeName, TMessage message)
            where TMessage : class, IBusMessage
        {
            publisher.PublishAsync(exchangeName, message).Wait();
        }

        private static void PublishAsync<TMessage>(IBusPublisher publisher, string exchangeName, string topic, TMessage message)
            where TMessage : class, IBusMessage
        {
            publisher.PublishAsync(exchangeName, topic, message).Wait();
        }

        private static void SendAsync<TMessage>(IBusPublisher publisher, string queueName, TMessage message)
            where TMessage : class, IBusMessage
        {
            publisher.SendAsync(queueName, message).Wait();
        }

        #region Mocks

        public string GetExchangeName()
        {
            return "TEST";
        }

        public string GetQueueName()
        {
            return "TEST";
        }

        public string GetTopic()
        {
            return "TEST";
        }

        public Message GetMessage()
        {
            return new Message();
        }

        public class Message : IBusMessage
        {
            public Message()
            {
                MessageId = Guid.NewGuid().ToString();
                ContentName = null;
            }

            public string MessageId { get; }
            public string ContentName { get; }
        }

        #endregion Mocks
    }
}