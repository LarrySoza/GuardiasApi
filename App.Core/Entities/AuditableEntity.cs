namespace App.Core.Entities
{
    // Campos base de auditoría usados en muchas tablas (created_at, created_by, updated_at, updated_by, deleted_at)
    public abstract class AuditableEntity<TId> : Entity<TId>
    {
        public DateTimeOffset? created_at { get; protected set; }
        public Guid? created_by { get; protected set; }
        public DateTimeOffset? updated_at { get; protected set; }
        public Guid? updated_by { get; protected set; }
        public DateTimeOffset? deleted_at { get; protected set; }
    }
}
