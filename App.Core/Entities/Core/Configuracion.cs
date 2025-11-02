namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: configuracion
    /// Valores de configuración por tipo.
    /// </summary>
    public class Configuracion : Entity<string>
    {
        public string valor { get; private set; } = string.Empty;

        protected Configuracion() { }
        public Configuracion(string id, string valor)
        {
            this.id = id;
            this.valor = valor;
        }
    }
}
