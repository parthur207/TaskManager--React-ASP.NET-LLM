using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.ExternalServices.Messaging
{
    public sealed class RabbitMqConnectionProvider : IDisposable
    {
        private readonly IConnection _connection;
        public RabbitMqConnectionProvider(IOptions<RabbitMqSettings> options)
        {
            var settings = options.Value;

            var factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
                VirtualHost = settings.VirtualHost,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true
            };

            _connection = factory.CreateConnection();
        }

        public IConnection Connection => _connection;

        public void Dispose() => _connection?.Dispose();
    }
}
