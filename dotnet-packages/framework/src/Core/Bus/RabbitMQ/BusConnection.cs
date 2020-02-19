using Framework.Core.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace Framework.Core.Bus.RabbitMQ
{
    public class BusConnection : IBusConnection
    {
        private readonly string _connectionString;
        private IConnection _connection;

        public BusConnection(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IConnection GetConnection()
        {
            if (_connection != null) return _connection;

            _connection = Policy.Handle<BrokerUnreachableException>()
                .WaitAndRetry(5,
                    (x) => TimeSpan.FromSeconds(5),
                    (e, t) => LogHelper.Debug($"Unable to connect to RabbitMQ ({_connectionString}). Message: {e.Message}). Retrying..."))
                .Execute(() =>
                {
                    var factory = GetConnectionFactory(_connectionString);
                    return factory.CreateConnection();
                });

            return _connection;
        }

        private static IConnectionFactory GetConnectionFactory(string connectionString)
        {
            var ret = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            return ret;
        }
    }
}