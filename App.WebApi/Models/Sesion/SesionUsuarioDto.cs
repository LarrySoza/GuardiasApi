using App.WebApi.Models.Common;

namespace App.WebApi.Models.Sesion
{
    /// <summary>
    /// DTO que expone la información de una sesión de usuario incluyendo auditoría.
    /// </summary>
    public class SesionUsuarioDto : AuditableEntityDto
    {
        public Guid id { get; set; }
        public Guid usuario_id { get; set; }
        public Guid? cliente_id { get; set; }
        public Guid unidad_id { get; set; }
        public Guid? puesto_id { get; set; }
        public int? turno_id { get; set; }
        public DateTimeOffset fecha_inicio { get; set; }
        public string? ruta_foto_inicio { get; set; }
        public DateTimeOffset? fecha_fin { get; set; }
        public object? equipos_a_cargo { get; set; }
        public string? otros_detalle { get; set; }
        public string? descripcion_cierre { get; set; }
    }
}