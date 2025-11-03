using System.Security.Cryptography;

namespace App.Core.Entities.Core
{
    /// <summary>
    /// Tabla: rol
    /// Catálogo de roles del sistema (GUARDIA, SUPERVISOR, ADMIN, ...)
    /// </summary>
    public class Rol
    {
        public required string id { get; set; }
        public string? nombre { get; set; }
    }
}
