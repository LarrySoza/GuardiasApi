using System.Collections.Concurrent;

namespace App.WebApi.Models
{
    // Servicio singleton: mantiene userId -> set de connectionIds
    public class ConnectedUsersTracker
    {
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, byte>> _map = new();
        // Conjunto de usuarios que pertenecen a UsersGroup (usuarios no administradores)
        private readonly ConcurrentDictionary<Guid, byte> _usersGroup = new();

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
                {
                    _map.TryRemove(userId, out _);
                    // Si ya no tiene conexiones, quitar también del UsersGroup por si quedó marcado
                    _usersGroup.TryRemove(userId, out _);
                }
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

        // UsersGroup management
        public void AddUserToUsersGroup(Guid userId)
        {
            _usersGroup[userId] = 0;
        }

        public void RemoveUserFromUsersGroup(Guid userId)
        {
            _usersGroup.TryRemove(userId, out _);
        }

        public IReadOnlyCollection<Guid> GetOnlineUsersNonAdmins()
        {
            // Devuelve solo los usuarios marcados en usersGroup que además estén efectivamente online
            var list = _usersGroup.Keys.Where(id => _map.ContainsKey(id)).ToList();
            return list.AsReadOnly();
        }
    }
}
