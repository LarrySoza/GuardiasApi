namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: round
    /// Record of rounds (control points or GPS readings) made during a session.
    /// </summary>
    public class Ronda : AuditableEntity<Guid>
    {
        public Guid? sesion_usuario_id { get; private set; }
        public Guid? control_point_id { get; private set; }
        public DateTimeOffset fecha_hora { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }
        public string? descripcion { get; private set; }

        private readonly List<RondaAdjunto> _adjuntos = new();
        public IReadOnlyCollection<RondaAdjunto> ronda_adjuntos => _adjuntos.AsReadOnly();

        protected Ronda() { }
        public Ronda(Guid id)
        {
            this.id = id;
            fecha_hora = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
