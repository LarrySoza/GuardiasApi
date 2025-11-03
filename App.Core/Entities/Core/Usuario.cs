namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: usuario
    /// </summary>
    public class Usuario : AuditableEntity<Guid>
    {
        public string? nombre_usuario { get; set; }
        public string? email { get; set; }
        public string? contrasena_hash { get; set; }
        public Guid sello_seguridad { get; set; }
        public string? telefono { get; set; }
        public string? tipo_documento_id { get; set; }
        public string? numero_documento { get; set; }
        public char estado { get; set; } = 'A';
    }
}
