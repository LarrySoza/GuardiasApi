namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: client
    /// Clients (companies). `user_id` is the responsible user.
    /// </summary>
    public class Cliente : AuditableEntity<Guid>
    {
        public string? razon_social { get; set; }
        public string? ruc { get; set; }
    }
}
