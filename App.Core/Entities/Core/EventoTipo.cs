namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: ocurrencia_tipo
    /// Catálogo de tipos de ocurrencias (global o por cliente).
    /// </summary>
    public class EventoTipo : AuditableEntity<Guid>
    {
        public Guid? cliente_id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public required string categoria_id { get; set; }
    }
}
