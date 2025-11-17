using App.Core.Entities.Core;

namespace App.Application.Models.PanicAlert
{
    /// <summary>
    /// DTO returned by paginated queries for panic alert notifications including basic user info.
    /// </summary>
    public class PanicAlertNotificacionDto : PanicAlertNotificacion
    {
        // Fields from usuario (receiver)
        public string? nombre_usuario { get; set; }
        public string? tipo_documento { get; set; }
        public string? numero_documento { get; set; }
        public string? nombre_completo { get; set; }
    }
}
