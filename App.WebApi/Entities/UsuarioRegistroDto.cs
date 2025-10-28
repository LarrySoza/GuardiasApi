namespace App.WebApi.Entities
{
    public class UsuarioRegistroDto
    {
        public required string nombre_usuario { get; set; }
        public required string clave { get; set; }
        public required string nombres { get; set; }
        public required string apellidos { get; set; }
        public required string tipo_documento { get; set; }
        public required string numero_documento { get; set; }
        public Guid? area_id { get; set; }
    }
}
