namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: user
    /// Users of the system with credentials and auditing.
    /// </summary>
    public class Usuario : AuditableEntity<Guid>
    {
        public string nombre_usuario { get; private set; } = string.Empty;
        public string? email { get; private set; }
        public string contrasena_hash { get; private set; } = string.Empty;
        public Guid sello_seguridad { get; private set; }
        public string? telefono { get; private set; }
        public string? tipo_documento { get; private set; }
        public string? numero_documento { get; private set; }
        public char estado { get; private set; } = 'A';

        private readonly List<UsuarioRol> _roles = new();
        public IReadOnlyCollection<UsuarioRol> usuario_rols => _roles.AsReadOnly();

        protected Usuario() { }
        public Usuario(Guid id, string nombreUsuario, string contrasenaHash)
        {
            this.id = id;
            nombre_usuario = nombreUsuario;
            contrasena_hash = contrasenaHash;
            sello_seguridad = Guid.NewGuid();
            created_at = DateTimeOffset.UtcNow;
        }

        public void CambiarContrasena(string nuevoHash)
        {
            contrasena_hash = nuevoHash;
            sello_seguridad = Guid.NewGuid();
            updated_at = DateTimeOffset.UtcNow;
        }

        public void AddRol(UsuarioRol usuarioRol)
        {
            if (usuarioRol == null) throw new ArgumentNullException(nameof(usuarioRol));
            _roles.Add(usuarioRol);
        }
    }
}
