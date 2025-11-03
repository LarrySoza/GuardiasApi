namespace App.WebApi.Models.Sesion
{
    public class CrearSesionRequestDto
    {
        public required Guid unidad_id { get; set; }
        public Guid? cliente_id { get; set; }
        public DateTimeOffset? fecha_inicio { get; set; }
        public string? otros_detalle { get; set; }

        // foto enviada desde el cliente (multipart/form-data)
        public IFormFile? foto_inicio { get; set; }
    }
}
