namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: unit
    /// Units or locations belonging to a client. Allows hierarchy.
    /// </summary>
    public class Unidad : AuditableEntity<Guid>
    {
        public Guid? cliente_id { get; private set; }
        public Guid? unidad_id_padre { get; private set; }
        public string? nombre { get; private set; }
        public string? direccion { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }

        private readonly List<UsuarioUnidad> _usuarioUnidades = new();
        public IReadOnlyCollection<UsuarioUnidad> usuario_unidades => _usuarioUnidades.AsReadOnly();

        protected Unidad() { }
        public Unidad(Guid id)
        {
            this.id = id;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
