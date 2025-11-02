namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: incidencia
    /// Incidentes reportados en el sistema.
    /// </summary>
    public class Incidencia : AuditableEntity<Guid>
    {
        public Guid? sesion_usuario_id { get; private set; }
        public Guid? ronda_id { get; private set; }
        public Guid incidente_tipo_id { get; private set; }
        public Guid puesto_id { get; private set; }
        public DateTimeOffset fecha_hora { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }
        public string? descripcion { get; private set; }

        private readonly List<IncidenciaAdjunto> _adjuntos = new();
        public IReadOnlyCollection<IncidenciaAdjunto> incidencia_adjuntos => _adjuntos.AsReadOnly();

        protected Incidencia() { }
        public Incidencia(Guid id, Guid incidenteTipoId, Guid puestoId)
        {
            this.id = id;
            incidente_tipo_id = incidenteTipoId;
            puesto_id = puestoId;
            fecha_hora = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
