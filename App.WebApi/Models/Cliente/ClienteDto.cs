using App.WebApi.Models.Common;

namespace App.WebApi.Models.Cliente
{
    public class ClienteDto : AuditableEntityDto
    {
        public Guid id { get; set; }
        public string? razon_social { get; set; }
        public string? ruc { get; set; }
    }
}
