namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_adjunto_tipo
    /// Catalog of attachment types for panic alerts.
    /// </summary>
    public class PanicAlertAdjuntoTipo
    {
        public required string id { get; set; }
        public string? nombre { get; set; }
    }
}
