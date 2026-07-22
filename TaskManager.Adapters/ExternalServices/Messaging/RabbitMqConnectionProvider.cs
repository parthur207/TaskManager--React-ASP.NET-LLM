using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TaskManager.Adapters.ExternalServices.Messaging
{
    public sealed class RabbitMqConnectionProvider : IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public RabbitMqConnectionProvider(IOptions<RabbitMqSettings> options)
        {
            var settings = options.Value;
            _factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
                VirtualHost = settings.VirtualHost,
                AutomaticRecoveryEnabled = true
            };
        }

        public async Task<IConnection> GetConnectionAsync(CancellationToken ct = default)
        {
            if (_connection is { IsOpen: true }) return _connection;

            await _lock.WaitAsync(ct);
            try
            {
                if (_connection is { IsOpen: true }) return _connection;
                _connection = await _factory.CreateConnectionAsync(ct);
                return _connection;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}