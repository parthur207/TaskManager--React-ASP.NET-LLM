using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskManager.Core.Ports.Messaging;

namespace TaskManager.Adapters.ExternalServices.Messaging.RabbitMQ
{
    public class RabbitMqPublisherAdapter : IMessagePublisherPort
    {
        private readonly RabbitMqConnectionProvider _connectionProvider;
        private readonly RabbitMqSettings _settings;

        public RabbitMqPublisherAdapter(
            RabbitMqConnectionProvider connectionProvider,
            IOptions<RabbitMqSettings> options)
        {
            _connectionProvider = connectionProvider;
            _settings = options.Value;
        }

        public Task PublishAsync<T>(T message, string routingKey) where T : class
        {
            using var channel = _connectionProvider.Connection.CreateModel();

            channel.ExchangeDeclare(_settings.ExchangeName, ExchangeType.Topic, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            channel.BasicPublish(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            return Task.CompletedTask;
        }
    }
}