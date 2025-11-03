namespace App.WebApi.Models.Unidad
{
    public class CrearUnidadRequestDto
    {
        public Guid cliente_id { get; set; }
        public Guid? unidad_id_padre { get; set; }
        public string nombre { get; set; } = default!;
        public string? direccion { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}