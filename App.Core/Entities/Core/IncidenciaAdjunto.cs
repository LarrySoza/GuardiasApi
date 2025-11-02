namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: incidencia_adjunto
    /// Files/attachments (photos, evidence) associated with an incidence.
    /// </summary>
    public class IncidenciaAdjunto : AuditableEntity<Guid>
    {
        public Guid incidencia_id { get; private set; }
        public string? ruta_foto { get; private set; }

        protected IncidenciaAdjunto() { }
        public IncidenciaAdjunto(Guid id, Guid incidenciaId, string rutaFoto)
        {
            this.id = id;
            incidencia_id = incidenciaId;
            ruta_foto = rutaFoto;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
