namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: ocurrencia
    /// Registros de ocurrencias asociadas a sesiones y puestos.
    /// </summary>
    public class Ocurrencia : AuditableEntity<Guid>
    {
        public Guid? sesion_usuario_id { get; private set; }
        public Guid? puesto_id { get; private set; }
        public Guid ocurrencia_tipo_id { get; private set; }
        public string? descripcion { get; private set; }
        public DateTimeOffset fecha_hora { get; private set; }

        private readonly List<OcurrenciaAdjunto> _adjuntos = new();
        public IReadOnlyCollection<OcurrenciaAdjunto> ocurrencia_adjuntos => _adjuntos.AsReadOnly();

        protected Ocurrencia() { }
        public Ocurrencia(Guid id, Guid ocurrenciaTipoId)
        {
            this.id = id;
            ocurrencia_tipo_id = ocurrenciaTipoId;
            fecha_hora = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
