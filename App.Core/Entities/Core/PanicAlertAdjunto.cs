namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_adjunto
    /// Files (image/audio/text) associated with a panic_alert.
    /// </summary>
    public class PanicAlertAdjunto : AuditableEntity<Guid>
    {
        public Guid panic_alert_id { get; set; }
        public string tipo_id { get; set; } = string.Empty;
        public string ruta { get; set; } = string.Empty;
    }
}
