namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: sesion_usuario
    /// Register user sessions (start/end), initial photo, equipment in charge, and closure.
    /// </summary>
    public class SesionUsuario : AuditableEntity<Guid>
    {
        public Guid usuario_id { get; private set; }
        public Guid? cliente_id { get; private set; }
        public Guid unidad_id { get; private set; }
        public DateTimeOffset fecha_inicio { get; private set; }
        public string? ruta_foto_inicio { get; private set; }
        public DateTimeOffset? fecha_fin { get; private set; }
        public EquiposACargo? equipos_a_cargo { get; private set; }
        public string? otros_detalle { get; private set; }
        public string? descripcion_cierre { get; private set; }

        public List<SesionUsuarioEvidencia> evidencias { get; set; } = new();
    }
}
