namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: asignacion
    /// Tareas/Asignaciones asociadas a un cliente/unidad.
    /// </summary>
    public class Asignacion : AuditableEntity<Guid>
    {
        public Guid? cliente_id { get; private set; }
        public Guid? unidad_id { get; private set; }
        public string? codigo { get; private set; }
        public string? tipo { get; private set; }
        public string? direccion { get; private set; }
        public string? descripcion { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }
        public Guid? usuario_asignado_id { get; private set; }
        public string? estado { get; private set; }
        public DateTimeOffset fecha_creacion { get; private set; }

        private readonly List<AsignacionEvento> _eventos = new();
        public IReadOnlyCollection<AsignacionEvento> asignacion_eventos => _eventos.AsReadOnly();

        protected Asignacion() { }
        public Asignacion(Guid id)
        {
            this.id = id;
            fecha_creacion = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
