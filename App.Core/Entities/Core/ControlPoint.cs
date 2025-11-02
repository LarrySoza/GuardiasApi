namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: control_point
    /// Control points (QR/GPS) associated with client and unit.
    /// </summary>
    public class ControlPoint : AuditableEntity<Guid>
    {
        public Guid cliente_id { get; private set; }
        public Guid unidad_id { get; private set; }
        public string nombre { get; private set; } = string.Empty;
        public string? codigo_qr { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }
        public int? radio { get; private set; }

        private readonly List<Ronda> _rondas = new();
        public IReadOnlyCollection<Ronda> rondas => _rondas.AsReadOnly();

        protected ControlPoint() { }
        public ControlPoint(Guid id, Guid clienteId, Guid unidadId, string nombre)
        {
            this.id = id;
            cliente_id = clienteId;
            unidad_id = unidadId;
            this.nombre = nombre;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
