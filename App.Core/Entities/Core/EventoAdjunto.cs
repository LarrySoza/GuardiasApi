namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: ocurrencia_adjunto
    /// Archivos/adjuntos (fotos, evidencias) asociados a una ocurrencia.
    /// </summary>
    public class EventoAdjunto : AuditableEntity<Guid>
    {
        public Guid evento_id { get; set; }
        public string adjunto_tipo_id { get; set; } = string.Empty;
        public string ruta { get; set; } = string.Empty;
    }
}
