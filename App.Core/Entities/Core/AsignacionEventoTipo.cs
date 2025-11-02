namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: asignacion_evento_tipo
    /// Catálogo de tipos de acción para eventos de asignación.
    /// </summary>
    public class AsignacionEventoTipo : Entity<string>
    {
        public string nombre { get; private set; } = string.Empty;

        protected AsignacionEventoTipo() { }
        public AsignacionEventoTipo(string id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }
}
