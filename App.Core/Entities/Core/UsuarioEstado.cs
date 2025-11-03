namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: usuario_estado
    /// Catalog of user states ('A','I','E').
    /// </summary>
    public class UsuarioEstado
    {
        public required string id { get; set; }
        public string? nombre { get; set; }
    }
}
