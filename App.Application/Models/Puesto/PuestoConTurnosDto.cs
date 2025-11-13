using App.Application.Models.Turno;

namespace App.Application.Models.Puesto
{
    public class PuestoConTurnosDto
    {
        public Guid id { get; set; }
        public Guid unidad_id { get; set; }
        public string? nombre { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }

        public List<TurnoDto> Turnos { get; set; } = new();
    }
}
