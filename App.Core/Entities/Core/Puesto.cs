namespace App.Core.Entities.Core
{
    /// <summary>
    /// Table: puesto
    /// Positions within a unit (e.g., guards assigned to a physical position).
    /// </summary>
    public class Puesto : AuditableEntity<Guid>
    {
        public Guid unidad_id { get; private set; }
        public string? nombre { get; private set; }
        public decimal? lat { get; private set; }
        public decimal? lng { get; private set; }
    }
}
