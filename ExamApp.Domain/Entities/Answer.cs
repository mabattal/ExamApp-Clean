using ExamApp.Domain.Entities.Common;

namespace ExamApp.Domain.Entities
{
    public class Answer : IAuditEntity
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string? SelectedAnswer { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
