using App.WebApi.Models.Common;

namespace App.WebApi.Models.Cliente
{
    /// <summary>
    /// DTO que expone la información del cliente.
    /// </summary>
    public class ClienteDto : AuditableEntityDto
    {
        public Guid id { get; set; }
        public string? razon_social { get; set; }
        public string? ruc { get; set; }
    }
}
