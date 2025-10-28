using App.WebApi.Controllers;
using App.WebApi.Entities;
using App.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace App.WebApi.Hubs
{
    [Authorize]
    public class TrackerHub : Hub
    {
        private readonly ConnectedUsersTracker _tracker;
        private readonly ILogger<TrackerHub> _logger;
        private readonly IConfiguration _config;
        private const string AdministratorsGroup = "Administradores";
        private const string UsersGroup = "UsersGroup";

        public TrackerHub(IConfiguration config, ILogger<TrackerHub> logger, ConnectedUsersTracker tracker)
        {
            _config = config;
            _logger = logger;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User!.Id();
                _tracker.AddConnection(userId, Context.ConnectionId);
                _logger.LogInformation("Usuario {UserId} conectado con ConnectionId {ConnectionId}.", userId, Context.ConnectionId);

                // Si el usuario tiene el rol Administrador, lo añadimos al grupo de administradores
                if (Context.User!.IsInRole("Administrador"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, AdministratorsGroup);
                    _logger.LogInformation("ConnectionId {ConnectionId} añadido al grupo {Group}.", Context.ConnectionId, AdministratorsGroup);
                }
                else
                {
                    // Usuario no administrador: añadir al UsersGroup y marcar en el tracker
                    await Groups.AddToGroupAsync(Context.ConnectionId, UsersGroup);
                    _tracker.AddUserToUsersGroup(userId);
                    _logger.LogInformation("ConnectionId {ConnectionId} añadido al grupo {Group}.", Context.ConnectionId, UsersGroup);

                    // Notificar a administradores que un usuario (no administrador) se ha conectado
                    await Clients.Group(AdministratorsGroup).SendAsync("ReceiveUserConnected", userId);
                }
            }
            catch (Exception ex)
            {
                // Si no hay claim de usuario, no añadimos nada (opcional: registrar)
                _logger.LogWarning(ex, "OnConnectedAsync: no se encontró claim de usuario para ConnectionId {ConnectionId}.", Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.User!.Id();
                _tracker.RemoveConnection(userId, Context.ConnectionId);

                // Quitar conexión de ambos grupos por seguridad
                try
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdministratorsGroup);
                    _logger.LogInformation("ConnectionId {ConnectionId} eliminado del grupo {Group}.", Context.ConnectionId, AdministratorsGroup);
                }
                catch (Exception exGroup)
                {
                    _logger.LogDebug(exGroup, "Error al quitar ConnectionId {ConnectionId} del grupo {Group}.", Context.ConnectionId, AdministratorsGroup);
                }

                try
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, UsersGroup);
                    _logger.LogInformation("ConnectionId {ConnectionId} eliminado del grupo {Group}.", Context.ConnectionId, UsersGroup);
                }
                catch (Exception exGroup)
                {
                    _logger.LogDebug(exGroup, "Error al quitar ConnectionId {ConnectionId} del grupo {Group}.", Context.ConnectionId, UsersGroup);
                }

                // Si ya no tiene conexiones, el tracker habrá quitado al usuario de usersGroup en RemoveConnection.
                if (exception is null)
                {
                    _logger.LogInformation("Usuario {UserId} desconectado (ConnectionId: {ConnectionId}).", userId, Context.ConnectionId);
                }
                else
                {
                    _logger.LogInformation(exception, "Usuario {UserId} desconectado con excepción (ConnectionId: {ConnectionId}).", userId, Context.ConnectionId);
                }

                // Notificar a administradores que un usuario (no administrador) se ha desconectado
                // Solo enviar si el usuario estaba marcado en UsersGroup (y por tanto es usuario no admin)
                var nonAdmins = _tracker.GetOnlineUsersNonAdmins();
                if (!nonAdmins.Contains(userId))
                {
                    // Si no está en la colección de nonAdmins significa que o bien nunca fue marcado
                    // o ya quedó desconectado (en ese caso notificamos la desconexión)
                    await Clients.Group(AdministratorsGroup).SendAsync("ReceiveUserDisconnected", userId);
                }
                else
                {
                    // Si sigue en nonAdmins (es inusual) no enviamos; normalmente RemoveConnection habrá quitado
                    // la marca cuando se quedó sin conexiones.
                    await Clients.Group(AdministratorsGroup).SendAsync("ReceiveUserDisconnected", userId);
                }
            }
            catch (Exception ex)
            {
                // Ignorar si no hay claim
                _logger.LogWarning(ex, "OnDisconnectedAsync: no se encontró claim de usuario para ConnectionId {ConnectionId}.", Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        [Authorize(Roles = "Administrador")]
        public Task<IReadOnlyCollection<Guid>> GetConnectedUsers()
        {
            // Devuelve solo los usuarios conectados que pertenecen al UsersGroup (no administradores)
            return Task.FromResult(_tracker.GetOnlineUsersNonAdmins());
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
                _logger.LogInformation("Usuario {UserId} actualizó ubicación (ConnectionId: {ConnectionId}) -> Lat: {Lat}, Lon: {Lon}.", userId, Context.ConnectionId, lat, lon);

                // Enviar solo a administradores (grupo)
                await Clients.Group(AdministratorsGroup).SendAsync("ReceiveLocation", userId, lat, lon);
            }
            catch (Exception ex)
            {
                // Si no hay claim de usuario, no añadimos nada (opcional: registrar)
                _logger.LogWarning(ex, "UpdateLocation: no se encontró claim de usuario o error procesando la ubicación para ConnectionId {ConnectionId}.", Context.ConnectionId);
            }
        }

        public async Task SendAlert(string message, string? title = null, string level = "Info")
        {
            try
            {
                var userId = Context.User!.Id();
                _tracker.AddConnection(userId, Context.ConnectionId);
                _logger.LogInformation("Usuario {UserId} envió alerta (ConnectionId: {ConnectionId}) -> Nivel: {Level}, Título: {Title}.",
                    userId, Context.ConnectionId, level, title);

                // Enviar solo a administradores (grupo)
                await Clients.Group(AdministratorsGroup).SendAsync("ReceiveAlert", userId, title, message, level);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SendAlert: no se encontró claim de usuario o error procesando la alerta para ConnectionId {ConnectionId}.", Context.ConnectionId);
            }
        }

        [Authorize(Roles = "Administrador")]
        public async Task SendNotification(Guid targetUserId, string message, string? title = null, string level = "Info")
        {
            try
            {
                var senderId = Context.User!.Id();
                _tracker.AddConnection(senderId, Context.ConnectionId);
                _logger.LogInformation("Usuario {UserId} enviando notificación a {TargetUserId} (ConnectionId: {ConnectionId}) -> Nivel: {Level}, Título: {Title}.",
                    senderId, targetUserId, Context.ConnectionId, level, title);

                var connections = _tracker.GetConnections(targetUserId);
                if (connections == null || connections.Count == 0)
                {
                    _logger.LogInformation("No se encontraron conexiones activas para el usuario {TargetUserId}.", targetUserId);
                    return;
                }

                // Notificar solo a las conexiones activas del usuario objetivo
                await Clients.Clients(connections).SendAsync("ReceiveNotification", senderId, title, message, level, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SendNotification: error notificando al usuario {TargetUserId} desde ConnectionId {ConnectionId}.", targetUserId, Context.ConnectionId);
            }
        }
    }
}
