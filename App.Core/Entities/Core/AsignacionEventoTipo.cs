namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: asignacion_evento_tipo
    /// Catálogo de tipos de acción para eventos de asignación.
    /// </summary>
    public class AsignacionEventoTipo
    {
        public required string id { get; set; }
        public string? nombre { get; set; }
    }
}
