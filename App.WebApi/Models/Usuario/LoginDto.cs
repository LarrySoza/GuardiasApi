namespace App.WebApi.Models.Usuario
{
    public class LoginDto
    {
        /// <summary>
        /// Por lo general se usa un correo electronico
        /// </summary>
        public required string usuario { get; set; }

        /// <summary>
        /// La clave de usuario sin encriptar
        /// </summary>
        public required string clave { get; set; }
    }
}
