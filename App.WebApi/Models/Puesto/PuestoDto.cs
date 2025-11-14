namespace App.WebApi.Models.Puesto
{
    /// <summary>
    /// DTO público que representa un Puesto.
    /// </summary>
    public class PuestoDto
    {
        public Guid id { get; set; }
        public Guid unidad_id { get; set; }
        public string? nombre { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}
