namespace App.WebApi.Models.Unidad
{
    /// <summary>
    /// DTO para crear una nueva unidad.
    /// </summary>
    public class CrearUnidadRequestDto
    {
        public Guid cliente_id { get; set; }
        public Guid? unidad_id_padre { get; set; }
        public string nombre { get; set; } = default!;
        public string? direccion { get; set; }
    }
}