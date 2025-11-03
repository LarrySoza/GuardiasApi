namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_estado
    /// Catalog of possible states for a panic alert.
    /// </summary>
    public class PanicAlertEstado
    {
        public required string id { get; set; }
        public string? nombre { get; set; }
    }
}
