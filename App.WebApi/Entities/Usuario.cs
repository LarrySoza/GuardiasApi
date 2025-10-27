namespace App.WebApi.Entities
{
    public class Usuario
    {
        public Guid usuario_id { get; set; }
        public DateTime fecha_registro { get; set; }
        public required string nombre_usuario { get; set; }
        public required string clave_hash { get; set; }
        public string? correo { get; set; }
        public bool correo_confirmado { get; set; }
        public Guid sello_seguridad { get; set; }
        public bool bloqueado { get; set; }
        public DateTime? fecha_desbloqueo { get; set; }
        public int contador_error_clave { get; set; }
        public bool activo { get; set; }
    }
}
