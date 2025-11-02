namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: alive_check
    /// Mensajes de verificación "Hombre Vivo" enviados.
    /// </summary>
    public class AliveCheck : AuditableEntity<Guid>
    {
        public Guid usuario_emisor_id { get; private set; }
        public Guid? sesion_usuario_dest_id { get; private set; }
        public DateTimeOffset fecha_hora_envio { get; private set; }
        public int timeout_seg { get; private set; }
        public string estado_id { get; private set; } = "01";

        private readonly List<AliveCheckRespuesta> _respuestas = new();
        public IReadOnlyCollection<AliveCheckRespuesta> alive_check_respuestas => _respuestas.AsReadOnly();

        protected AliveCheck() { }
        public AliveCheck(Guid id, Guid usuarioEmisorId)
        {
            this.id = id;
            usuario_emisor_id = usuarioEmisorId;
            fecha_hora_envio = DateTimeOffset.UtcNow;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
