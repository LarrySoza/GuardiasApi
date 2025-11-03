namespace App.WebApi.Models.Auth
{
    public class LoginRequestDto
    {
        /// <summary>
        /// Por lo general se usa un correo electronico
        /// </summary>
        public required string nombre_usuario { get; set; }

        /// <summary>
        /// La clave de usuario sin encriptar
        /// </summary>
        public required string contrasena { get; set; }
    }
}
