namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: alive_check_estado
    /// Catalog of states for alive_check.
    /// </summary>
    public class AliveCheckEstado : Entity<string>
    {
        public string nombre { get; private set; } = string.Empty;

        protected AliveCheckEstado() { }
        public AliveCheckEstado(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
