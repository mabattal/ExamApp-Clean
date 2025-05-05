namespace ExamApp.Domain.Entities.Common
{
    public interface IAuditEntity
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
