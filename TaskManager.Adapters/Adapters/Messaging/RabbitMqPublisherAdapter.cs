using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskManager.Adapters.ExternalServices.Messaging;
using TaskManager.Core.Ports.Messaging;

namespace TaskManager.Adapters.Adapters.Messaging
{
    public class RabbitMqPublisherAdapter : IMessagePublisherPort
    {
        private readonly RabbitMqConnectionProvider _connectionProvider;
        private readonly RabbitMqSettings _settings;

        public RabbitMqPublisherAdapter(RabbitMqConnectionProvider connectionProvider, IOptions<RabbitMqSettings> options)
        {
            _connectionProvider = connectionProvider;
            _settings = options.Value;
        }

        public async Task<T> PublishAsync<T>(T message, string routingKey) where T : class
        {
            var connection = await _connectionProvider.GetConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await channel.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);

            return message;
        }
    }
}