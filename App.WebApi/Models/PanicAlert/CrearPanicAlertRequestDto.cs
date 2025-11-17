namespace App.WebApi.Models.PanicAlert
{
    /// <summary>
    /// DTO para crear una alerta de pánico.
    /// </summary>
    public class CrearPanicAlertRequestDto
    {
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}
