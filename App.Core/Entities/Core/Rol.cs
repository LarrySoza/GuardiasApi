namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: rol
    /// Catálogo de roles del sistema (GUARDIA, SUPERVISOR, ADMIN, ...)
    /// </summary>
    public class Rol : Entity<string>
    {
        public string? codigo { get; private set; }

        public string nombre { get; private set; } = string.Empty;

        // Si es necesario, navegación a usuarios vía la entidad de asociación `usuario_rol`
        private readonly List<UsuarioRol> _usuarioRols = new();
        public IReadOnlyCollection<UsuarioRol> usuario_rols => _usuarioRols.AsReadOnly();

        protected Rol() { }
        public Rol(string id, string codigo)
        {
            this.id = id;
            this.codigo = codigo;
        }
    }
}
