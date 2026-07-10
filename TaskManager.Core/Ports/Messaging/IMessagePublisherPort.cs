namespace TaskManager.Core.Ports.Messaging
{
    public interface IMessagePublisherPort
    {
        Task PublishAsync<T>(T message, string routingKey) where T : class;
    }
}
