namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: puesto
    /// Positions within a unit (e.g., guards assigned to a physical position).
    /// </summary>
    public class Puesto : AuditableEntity<Guid>
    {
        public Guid unidad_id { get; set; }
        public string? nombre { get; set; }
        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}
