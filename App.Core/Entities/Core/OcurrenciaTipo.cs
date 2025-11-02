namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: ocurrencia_tipo
    /// Catálogo de tipos de ocurrencias (global o por cliente).
    /// </summary>
    public class OcurrenciaTipo : AuditableEntity<Guid>
    {
        public Guid? cliente_id { get; private set; }
        public string nombre { get; private set; } = string.Empty;

        protected OcurrenciaTipo() { }
        public OcurrenciaTipo(Guid id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
