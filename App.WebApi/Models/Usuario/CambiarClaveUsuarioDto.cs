using System.ComponentModel.DataAnnotations;

namespace App.WebApi.Models.Usuario
{
    public class CambiarClaveUsuarioDto
    {
        [StringLength(20, MinimumLength = 5)]
        public required string NuevaClave { get; set; }
    }
}
