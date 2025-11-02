namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: rol
    /// Catálogo de roles del sistema (GUARDIA, SUPERVISOR, ADMIN, ...)
    /// </summary>
    public class Rol : Entity<string>
    {
        public string? codigo { get; private set; }

        protected Rol() { }
        public Rol(string id, string codigo)
        {
            this.id = id;
            this.codigo = codigo;
        }
    }
}
