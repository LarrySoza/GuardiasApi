using System.ComponentModel.DataAnnotations;

namespace App.WebApi.Entities
{
    public class CambiarClaveUsuarioDto
    {
        public required string UsuarioId { get; set; }

        [StringLength(20, MinimumLength = 5)]
        public required string NuevaClave { get; set; }
    }
}
