namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_estado
    /// Catalog of possible states for a panic alert.
    /// </summary>
    public class PanicAlertEstado : Entity<string>
    {
        public string nombre { get; private set; } = string.Empty;

        protected PanicAlertEstado() { }
        public PanicAlertEstado(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
