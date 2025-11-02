namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: usuario_rol
    /// N:N relationship between users and roles. It has audit fields.
    /// </summary>
    public class UsuarioRol
    {
        public Guid usuario_id { get; private set; }
        public string rol_id { get; private set; } = string.Empty;

        public DateTimeOffset? created_at { get; private set; }
        public Guid? created_by { get; private set; }
        public DateTimeOffset? updated_at { get; private set; }
        public Guid? updated_by { get; private set; }
        public DateTimeOffset? deleted_at { get; private set; }

        protected UsuarioRol() { }
        public UsuarioRol(Guid usuarioId, string rolId, Guid? createdBy = null)
        {
            usuario_id = usuarioId;
            rol_id = rolId;
            created_at = DateTimeOffset.UtcNow;
            created_by = createdBy;
        }
    }
}
