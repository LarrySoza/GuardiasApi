namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: ronda_adjunto
    /// Files/attachments (photos) associated with a round.
    /// </summary>
    public class RondaAdjunto : AuditableEntity<Guid>
    {
        public Guid ronda_id { get; private set; }
        public string? ruta { get; private set; }

        protected RondaAdjunto() { }
        public RondaAdjunto(Guid id, Guid rondaId, string ruta)
        {
            this.id = id;
            ronda_id = rondaId;
            this.ruta = ruta;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
