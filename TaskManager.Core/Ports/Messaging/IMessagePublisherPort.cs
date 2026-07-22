namespace TaskManager.Core.Ports.Messaging
{
    public interface IMessagePublisherPort
    {
        Task<T> PublishAsync<T>(T message, string routingKey) where T : class;
    }
}
