namespace App.WebApi.Models.Usuario
{
    public class CambiarClaveDto
    {
        public required string clave_actual { get; set; }
        public required string clave_nueva { get; set; }
    }
}
