namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: puesto
    /// Positions within a unit (e.g., guards assigned to a physical position).
    /// </summary>
    public class Puesto : AuditableEntity<Guid>
    {
        public Guid unidad_id { get; private set; }
        public string nombre { get; private set; } = string.Empty;

        private readonly List<Incidencia> _incidencias = new();
        public IReadOnlyCollection<Incidencia> incidencias => _incidencias.AsReadOnly();

        protected Puesto() { }
        public Puesto(Guid id, Guid unidadId, string nombre)
        {
            this.id = id;
            unidad_id = unidadId;
            this.nombre = nombre;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
