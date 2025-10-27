using System.ComponentModel.DataAnnotations;

namespace App.WebApi.Entities
{
    public class Login
    {
        /// <summary>
        /// Por lo general se usa un correo electronico
        /// </summary>
        [Required]
        public required string usuario { get; set; }

        /// <summary>
        /// La clave de usuario sin encriptar
        /// </summary>
        [Required]
        public required string clave { get; set; }
    }
}
