namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: tipo_configuracion
    /// Catalog of system configuration types.
    /// </summary>
    public class TipoConfiguracion : Entity<string>
    {
        public string nombre { get; private set; } = string.Empty;

        protected TipoConfiguracion() { }
        public TipoConfiguracion(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
