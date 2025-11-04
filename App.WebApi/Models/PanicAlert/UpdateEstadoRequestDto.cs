namespace App.WebApi.Models.PanicAlert
{
    /// <summary>
    /// DTO para actualizar el estado de una PanicAlert.
    /// </summary>
    public class UpdateEstadoRequestDto
    {
        public string? estado_id { get; set; }
    }
}
