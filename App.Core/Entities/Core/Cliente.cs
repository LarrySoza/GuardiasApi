namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: client
    /// Clients (companies). `user_id` is the responsible user.
    /// </summary>
    public class Cliente : AuditableEntity<Guid>
    {
        public string? razon_social { get; private set; }
        public string? ruc { get; private set; }

        private readonly List<ClienteUsuario> _clienteUsuarios = new();
        public IReadOnlyCollection<ClienteUsuario> cliente_usuarios => _clienteUsuarios.AsReadOnly();

        protected Cliente() { }
        public Cliente(Guid id)
        {
            this.id = id;
            created_at = DateTimeOffset.UtcNow;
        }
    }
}
