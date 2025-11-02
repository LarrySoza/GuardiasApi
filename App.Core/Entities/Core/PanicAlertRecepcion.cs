namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_recepcion
    /// Register the reception/attention of a panic alert by a receiving user.
    /// </summary>
    public class PanicAlertRecepcion : AuditableEntity<Guid>
    {
        public Guid panic_alert_id { get; private set; }
        public Guid usuario_receptor_id { get; private set; }
        public DateTimeOffset fecha_hora { get; private set; }

        protected PanicAlertRecepcion() { }
        public PanicAlertRecepcion(Guid id, Guid panicAlertId, Guid usuarioReceptorId)
        {
            this.id = id;
            panic_alert_id = panicAlertId;
            usuario_receptor_id = usuarioReceptorId;
            fecha_hora = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
