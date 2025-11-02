namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: cliente_usuario
    /// Relationship N:N between `cliente` and `usuario` with auditing.
    /// </summary>
    public class ClienteUsuario
    {
        public Guid cliente_id { get; private set; }
        public Guid usuario_id { get; private set; }

        public DateTimeOffset? created_at { get; private set; }
        public Guid? created_by { get; private set; }
        public DateTimeOffset? updated_at { get; private set; }
        public Guid? updated_by { get; private set; }
        public DateTimeOffset? deleted_at { get; private set; }

        protected ClienteUsuario() { }
        public ClienteUsuario(Guid clienteId, Guid usuarioId, Guid? createdBy = null)
        {
            cliente_id = clienteId;
            usuario_id = usuarioId;
            created_at = DateTimeOffset.UtcNow;
            created_by = createdBy;
        }
    }
}
