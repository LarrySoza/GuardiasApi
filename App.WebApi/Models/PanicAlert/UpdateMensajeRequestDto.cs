namespace App.WebApi.Models.PanicAlert
{
    /// <summary>
    /// DTO para la actualización del campo "mensaje" de una PanicAlert.
    /// </summary>
    public class UpdateMensajeRequestDto
    {
        public string? mensaje { get; set; }
    }
}
