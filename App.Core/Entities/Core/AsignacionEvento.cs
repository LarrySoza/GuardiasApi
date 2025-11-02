namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: asignacion_evento
    /// Eventos/acciones realizadas sobre una asignación.
    /// </summary>
    public class AsignacionEvento : AuditableEntity<Guid>
    {
        public Guid asignacion_id { get; private set; }
        public Guid? sesion_usuario_id { get; private set; }
        public DateTimeOffset? fecha_hora { get; private set; }
        public string accion_id { get; private set; } = "I";

        protected AsignacionEvento() { }
        public AsignacionEvento(Guid id, Guid asignacionId, string accionId)
        {
            this.id = id;
            asignacion_id = asignacionId;
            accion_id = accionId;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
