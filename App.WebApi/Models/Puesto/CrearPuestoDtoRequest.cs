namespace App.WebApi.Models.Puesto
{
    public class CrearPuestoDtoRequest
    {
        public Guid unidad_id { get; set; }
        public string? nombre { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
        public List<int>? turnos_ids { get; set; }
    }
}
