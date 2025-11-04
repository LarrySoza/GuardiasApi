namespace App.WebApi.Models.Unidad
{
    /// <summary>
    /// DTO para actualizar una unidad existente.
    /// </summary>
    public class ActualizarUnidadRequestDto
    {
        public Guid cliente_id { get; set; }
        public string nombre { get; set; } = default!;
        public string? direccion { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}