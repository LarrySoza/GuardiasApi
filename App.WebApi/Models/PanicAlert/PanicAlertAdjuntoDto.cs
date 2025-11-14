using App.WebApi.Models.Common;

namespace App.WebApi.Models.PanicAlert
{
    /// <summary>
    /// DTO que representa un adjunto de una PanicAlert.
    /// </summary>
    public class PanicAlertAdjuntoDto : AuditableEntityDto
    {
        public Guid id { get; set; }
        public Guid panic_alert_id { get; set; }
        public string adjunto_tipo_id { get; set; } = string.Empty;
        public string ruta { get; set; } = string.Empty;
    }
}
