using System.Collections.Concurrent;

namespace TaskManager.API.Hubs.RealTime
{
    /// <summary>
    /// Mapeia usuário -> connectionIds para poder retirar conexões de um grupo no servidor
    /// quando o usuário é removido do espaço. In-memory: vale para 1 instância da API.
    /// Com várias instâncias, troque por uma implementação em Redis.
    /// </summary>
    public interface IUserConnectionTracker
    {
        void Add(Guid userId, string connectionId);
        void Remove(Guid userId, string connectionId);
        IReadOnlyCollection<string> GetConnections(Guid userId);
    }

    public sealed class UserConnectionTracker : IUserConnectionTracker
    {
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, byte>> _connections = new();

        public void Add(Guid userId, string connectionId)
            => _connections.GetOrAdd(userId, _ => new()).TryAdd(connectionId, 0);

        public void Remove(Guid userId, string connectionId)
        {
            if (!_connections.TryGetValue(userId, out var set)) return;
            set.TryRemove(connectionId, out _);
            if (set.IsEmpty) _connections.TryRemove(userId, out _);
        }

        public IReadOnlyCollection<string> GetConnections(Guid userId)
            => _connections.TryGetValue(userId, out var set) ? set.Keys.ToArray() : Array.Empty<string>();
    }
}