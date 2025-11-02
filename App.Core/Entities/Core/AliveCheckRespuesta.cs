namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: alive_check_respuesta
    /// Respuestas a los alive_check por parte del guardia.
    /// </summary>
    public class AliveCheckRespuesta : AuditableEntity<Guid>
    {
        public Guid alive_check_id { get; private set; }
        public Guid? sesion_usuario_respuesta_id { get; private set; }
        public DateTimeOffset? fecha_hora_respuesta { get; private set; }
        public bool? aceptado { get; private set; }
        public string? ruta_foto { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }

        protected AliveCheckRespuesta() { }
        public AliveCheckRespuesta(Guid id, Guid aliveCheckId)
        {
            this.id = id;
            alive_check_id = aliveCheckId;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
