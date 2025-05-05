namespace ExamApp.Domain.Entities
{
    public class ExamResult
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? CompletionDate { get; set; }
        public int? Duration { get; set; }
        public int TotalQuestions { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? IncorrectAnswers { get; set; }
        public int? EmptyAnswers { get; set; }
        public decimal? Score { get; set; }
    }
}
