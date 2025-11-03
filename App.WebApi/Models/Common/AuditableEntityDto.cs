namespace App.WebApi.Models.Common
{
    // Campos base de auditoría usados por los DTOs
    public abstract class AuditableEntityDto
    {
        public DateTimeOffset? created_at { get; set; }
        public Guid? created_by { get; set; }
        public DateTimeOffset? updated_at { get; set; }
        public Guid? updated_by { get; set; }
        public DateTimeOffset? deleted_at { get; set; }
    }
}
