namespace App.WebApi.Entities
{
    public class VwUsuarioPerfil : UsuarioRegistroDto
    {
        public Guid usuario_id { get; set; }
        public string? nombre_area { get; set; }
        public DateTime fecha_registro { get; set; }
        public DateTime fecha_actualizacion { get; set; }
    }
}
