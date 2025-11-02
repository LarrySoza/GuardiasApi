namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: sesion_usuario_evidencia
    /// Evidence (photos) associated with a session.
    /// </summary>
    public class SesionUsuarioEvidencia : AuditableEntity<Guid>
    {
        public Guid sesion_usuario_id { get; private set; }
        public string? ruta_foto { get; private set; }

        protected SesionUsuarioEvidencia() { }
        public SesionUsuarioEvidencia(Guid id, Guid sesionUsuarioId, string rutaFoto)
        {
            this.id = id;
            sesion_usuario_id = sesionUsuarioId;
            ruta_foto = rutaFoto;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
