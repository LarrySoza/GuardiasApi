namespace App.WebApi.Models.PanicAlert
{
    public class CrearPanicAlertRequestDto
    {
        public required Guid sesion_usuario_id { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
        public DateTimeOffset? fecha_hora { get; set; }
    }
}
