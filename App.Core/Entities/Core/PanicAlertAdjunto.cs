namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_adjunto
    /// Files (image/audio/text) associated with a panic_alert.
    /// </summary>
    public class PanicAlertAdjunto : AuditableEntity<Guid>
    {
        public Guid panic_alert_id { get; private set; }
        public string tipo_id { get; private set; } = string.Empty;
        public string? ruta { get; private set; }

        protected PanicAlertAdjunto() { }
        public PanicAlertAdjunto(Guid id, Guid panicAlertId, string tipoId, string ruta)
        {
            this.id = id;
            panic_alert_id = panicAlertId;
            tipo_id = tipoId;
            this.ruta = ruta;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
