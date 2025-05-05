using ExamApp.Domain.Entities.Common;

namespace ExamApp.Domain.Entities
{
    public class Exam : IAuditEntity
    {
        public int ExamId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int Duration { get; set; } // Dakika cinsinden
        public int CreatedBy { get; set; } // Eğitmen ID
        public User Instructor { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
