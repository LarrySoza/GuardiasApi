namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: usuario_estado
    /// Catalog of user states ('A','I','E').
    /// </summary>
    public class UsuarioEstado : Entity<string>
    {
        public string nombre { get; private set; } = string.Empty;

        protected UsuarioEstado() { }
        public UsuarioEstado(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
