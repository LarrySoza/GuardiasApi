using App.WebApi.Models.Common;

namespace App.WebApi.Models.PanicAlert
{
    /// <summary>
    /// DTO que expone las propiedades de `PanicAlert` incluyendo campos de auditoría.
    /// </summary>
    public class PanicAlertDto : AuditableEntityDto
    {
        public Guid id { get; set; }
        public Guid? sesion_usuario_id { get; set; }
        public DateTimeOffset fecha_hora { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
        public string? mensaje { get; set; }
        public string estado_id { get; set; } = string.Empty;
    }
}
