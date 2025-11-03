namespace App.WebApi.Models.Usuario
{
    public class UsuarioDto
    {
        public Guid id { get; set; }
        public string nombre_usuario { get; set; } = default!;
        public string? email { get; set; }
        public string? telefono { get; set; }
        // otros campos públicos...
        public List<RolDto> roles { get; set; } = new();
    }
}
