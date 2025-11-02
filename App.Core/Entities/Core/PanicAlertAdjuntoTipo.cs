namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: panic_alert_adjunto_tipo
    /// Catalog of attachment types for panic alerts.
    /// </summary>
    public class PanicAlertAdjuntoTipo : Entity<string>
    {
        public string nombre { get; private set; } = string.Empty;

        protected PanicAlertAdjuntoTipo() { }
        public PanicAlertAdjuntoTipo(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
