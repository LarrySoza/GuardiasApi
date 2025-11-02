namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: ocurrencia_adjunto
    /// Archivos/adjuntos (fotos, evidencias) asociados a una ocurrencia.
    /// </summary>
    public class OcurrenciaAdjunto : AuditableEntity<Guid>
    {
        public Guid ocurrencia_id { get; private set; }
        public string? ruta_foto { get; private set; }

        protected OcurrenciaAdjunto() { }
        public OcurrenciaAdjunto(Guid id, Guid ocurrenciaId, string rutaFoto)
        {
            this.id = id;
            ocurrencia_id = ocurrenciaId;
            ruta_foto = rutaFoto;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
