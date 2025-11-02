namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert
    /// Records panic alerts linked to sessions.
    /// </summary>
    public class PanicAlert : AuditableEntity<Guid>
    {
        public Guid? sesion_usuario_id { get; private set; }
        public DateTimeOffset fecha_hora { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }
        public string? mensaje { get; private set; }
        public string? estado_id { get; private set; }

        private readonly List<PanicAlertAdjunto> _adjuntos = new();
        public IReadOnlyCollection<PanicAlertAdjunto> panic_alert_adjuntos => _adjuntos.AsReadOnly();

        protected PanicAlert() { }
        public PanicAlert(Guid id)
        {
            this.id = id;
            fecha_hora = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
