namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert
    /// Records panic alerts linked to sessions.
    /// </summary>
    public class PanicAlert : AuditableEntity<Guid>
    {
        public Guid sesion_usuario_id { get; set; }
        public DateTimeOffset fecha_hora { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
        public string? mensaje { get; set; }
        public string estado_id { get; set; } = "01";//ENVIADA
    }
}
