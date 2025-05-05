using ExamApp.Domain.Entities.Common;

namespace ExamApp.Domain.Entities
{
    public class Question : IAuditEntity
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public string QuestionText { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
