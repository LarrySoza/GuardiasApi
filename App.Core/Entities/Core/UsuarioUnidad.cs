namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: usuario_unidad
    /// Asignaciones de usuarios a unidades (N:N) con campos de auditoría.
    /// </summary>
    public class UsuarioUnidad
    {
        public Guid usuario_id { get; private set; }
        public Guid unidad_id { get; private set; }

        // Campos de auditoría (coinciden con columnas SQL)
        public DateTimeOffset? created_at { get; private set; }
        public Guid? created_by { get; private set; }
        public DateTimeOffset? updated_at { get; private set; }
        public Guid? updated_by { get; private set; }
        public DateTimeOffset? deleted_at { get; private set; }

        protected UsuarioUnidad() { }

        public UsuarioUnidad(Guid usuarioId, Guid unidadId, Guid? createdBy = null)
        {
            usuario_id = usuarioId;
            unidad_id = unidadId;
            created_at = DateTimeOffset.UtcNow;
            created_by = createdBy;
        }
    }
}
