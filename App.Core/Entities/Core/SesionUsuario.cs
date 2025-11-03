namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: sesion_usuario
    /// Register user sessions (start/end), initial photo, equipment in charge, and closure.
    /// </summary>
    public class SesionUsuario : AuditableEntity<Guid>
    {
        public Guid usuario_id { get; set; }
        public Guid? cliente_id { get; set; }
        public Guid unidad_id { get; set; }
        public DateTimeOffset fecha_inicio { get; set; }
        public string? ruta_foto_inicio { get; set; }
        public DateTimeOffset? fecha_fin { get; set; }
        public EquiposACargo? equipos_a_cargo { get; set; }
        public string? otros_detalle { get; set; }
        public string? descripcion_cierre { get; set; }

        public List<SesionUsuarioEvidencia> evidencias { get; set; } = new();
    }
}
