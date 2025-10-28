using System.Collections.Concurrent;

namespace App.WebApi.Entities
{
    // Servicio singleton: mantiene userId -> set de connectionIds
    public class ConnectedUsersTracker
    {
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, byte>> _map = new();

        public void AddConnection(Guid userId, string connectionId)
        {
            var set = _map.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());
            set[connectionId] = 0;
        }

        public void RemoveConnection(Guid userId, string connectionId)
        {
            if (_map.TryGetValue(userId, out var set))
            {
                set.TryRemove(connectionId, out _);
                if (set.IsEmpty)
                    _map.TryRemove(userId, out _);
            }
        }

        public IReadOnlyCollection<Guid> GetOnlineUsers()
        {
            return _map.Keys.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<string> GetConnections(Guid userId)
        {
            if (_map.TryGetValue(userId, out var set))
                return set.Keys.ToList().AsReadOnly();
            return Array.Empty<string>();
        }

        public IDictionary<Guid, IReadOnlyCollection<string>> GetAll()
        {
            return _map.ToDictionary(p => p.Key, p => (IReadOnlyCollection<string>)p.Value.Keys.ToList());
        }
    }
}
