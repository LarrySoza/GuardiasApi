namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: usuario_unidad
    /// Asignaciones de usuarios a unidades (N:N).
    /// </summary>
    public class UsuarioUnidad
    {
        public Guid usuario_id { get; set; }
        public Guid unidad_id { get; set; }
    }
}
