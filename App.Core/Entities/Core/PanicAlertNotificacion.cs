namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_notificacion
    /// Register the notification/attention of a panic alert to a receiving user.
    /// </summary>
    public class PanicAlertNotificacion : AuditableEntity<Guid>
    {
        public Guid panic_alert_id { get; set; }
        public Guid usuario_notificado_id { get; set; }
        public DateTimeOffset fecha_hora { get; set; }
        public bool aceptada { get; set; } = false;
    }
}
