using App.WebApi.Models.Common;

namespace App.WebApi.Models.Unidad
{
    public class UnidadDto : AuditableEntityDto
    {
        public Guid id { get; set; }
        public Guid? cliente_id { get; set; }
        public Guid? unidad_id_padre { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }

        // hijos para representar el árbol
        public List<UnidadDto> children { get; set; } = new();
    }
}