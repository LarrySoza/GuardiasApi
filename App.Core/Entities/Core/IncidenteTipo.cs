namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: incident_type
    /// Catalog of incident types (e.g. theft, accident, medical attention).
    /// </summary>
    public class IncidenteTipo : AuditableEntity<Guid>
    {
        public string nombre { get; private set; } = string.Empty;

        protected IncidenteTipo() { }
        public IncidenteTipo(Guid id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
