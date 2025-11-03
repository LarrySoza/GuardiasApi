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

        // Establece los campos de auditoría de creación
        public void SetCreationAudit(DateTimeOffset? dateTimeOffset, Guid? userId)
        {
            if (dateTimeOffset.HasValue) created_at = dateTimeOffset.Value;
            if (userId.HasValue) created_by = userId;
        }

        // Establece los campos de auditoría de actualización
        public void SetUpdateAudit(DateTimeOffset? dateTimeOffset, Guid? userId)
        {
            if (dateTimeOffset.HasValue) updated_at = dateTimeOffset.Value;
            if (userId.HasValue) updated_by = userId;
        }

        // Establece el campo de eliminación (soft delete)
        public void SetDeletedAudit(DateTimeOffset? dateTimeOffset)
        {
            if (dateTimeOffset.HasValue) deleted_at = dateTimeOffset.Value;
        }
    }
}
