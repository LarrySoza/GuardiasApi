namespace App.WebApi.Models.Cliente
{
    /// <summary>
    /// DTO para crear un cliente.
    /// </summary>
    public class CrearClienteRequestDto
    {
        public string? razon_social { get; set; }
        public string? ruc { get; set; }
    }
}