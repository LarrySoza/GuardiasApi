namespace App.WebApi.Entities
{
    public class UsuarioPerfil
    {
        public Guid usuario_id { get; set; }
        public required string nombres { get; set; }
        public required string apellidos { get; set; }
        public required string tipo_documento { get; set; }
        public required string numero_documento { get; set; }
        public Guid area_id { get; set; }
        public DateTime fecha_actualizacion { get; set; }
    }
}
