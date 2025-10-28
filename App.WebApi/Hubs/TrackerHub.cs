using App.WebApi.Controllers;
using App.WebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace App.WebApi.Hubs
{
    [Authorize]
    public class TrackerHub : Hub
    {
        private readonly ConnectedUsersTracker _tracker;
        private readonly ILogger<TrackerHub> _logger;
        private const string AdministratorsGroup = "Administradores";

        public TrackerHub(ConnectedUsersTracker tracker, ILogger<TrackerHub> logger)
        {
            _tracker = tracker;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User!.Id();
                _tracker.AddConnection(userId, Context.ConnectionId);
                _logger.LogInformation("User {UserId} connected with ConnectionId {ConnectionId}.", userId, Context.ConnectionId);

                // Si el usuario tiene el rol Administrador, lo añadimos al grupo de administradores
                if (Context.User!.IsInRole("Administrador"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, AdministratorsGroup);
                    _logger.LogInformation("ConnectionId {ConnectionId} added to group {Group}.", Context.ConnectionId, AdministratorsGroup);
                }
            }
            catch (Exception ex)
            {
                // Si no hay claim de usuario, no añadimos nada (opcional: registrar)
                _logger.LogWarning(ex, "OnConnectedAsync: no user claim found for ConnectionId {ConnectionId}.", Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.User!.Id();
                _tracker.RemoveConnection(userId, Context.ConnectionId);

                // Quitar conexión del grupo si estaba en él
                try
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdministratorsGroup);
                    _logger.LogInformation("ConnectionId {ConnectionId} removed from group {Group}.", Context.ConnectionId, AdministratorsGroup);
                }
                catch (Exception exGroup)
                {
                    // No fatal si no se puede quitar del grupo
                    _logger.LogDebug(exGroup, "Failed removing ConnectionId {ConnectionId} from group {Group}.", Context.ConnectionId, AdministratorsGroup);
                }

                if (exception is null)
                {
                    _logger.LogInformation("User {UserId} disconnected (ConnectionId: {ConnectionId}).", userId, Context.ConnectionId);
                }
                else
                {
                    _logger.LogInformation(exception, "User {UserId} disconnected with exception (ConnectionId: {ConnectionId}).", userId, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                // Ignorar si no hay claim
                _logger.LogWarning(ex, "OnDisconnectedAsync: no user claim found for ConnectionId {ConnectionId}.", Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        [Authorize(Roles = "Administrador")]
        public Task<IReadOnlyCollection<Guid>> GetConnectedUsers()
        {
            return Task.FromResult(_tracker.GetOnlineUsers());
        }

        [Authorize(Roles = "Administrador")]
        public Task<IReadOnlyCollection<string>> GetConnectionsFor(Guid userId)
        {
            return Task.FromResult(_tracker.GetConnections(userId));
        }

        public async Task UpdateLocation(double lat, double lon)
        {
            try
            {
                var userId = Context.User!.Id();
                _tracker.AddConnection(userId, Context.ConnectionId);
                _logger.LogInformation("User {UserId} updated location (ConnectionId: {ConnectionId}) -> Lat: {Lat}, Lon: {Lon}.", userId, Context.ConnectionId, lat, lon);

                // Enviar solo a administradores (grupo)
                await Clients.Group(AdministratorsGroup).SendAsync("ReceiveLocation", userId, lat, lon);
            }
            catch (Exception ex)
            {
                // Si no hay claim de usuario, no añadimos nada (opcional: registrar)
                _logger.LogWarning(ex, "UpdateLocation: no user claim found or error processing location for ConnectionId {ConnectionId}.", Context.ConnectionId);
            }
        }
    }
}
