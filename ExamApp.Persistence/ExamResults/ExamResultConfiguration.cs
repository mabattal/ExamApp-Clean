using ExamApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamApp.Persistence.ExamResults
{
    public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> builder)
        {
            builder.HasKey(er => er.ResultId);
            builder.Property(er => er.Score).HasPrecision(10, 2).IsRequired(false);
            builder.Property(er => er.StartDate).IsRequired();
            builder.Property(er => er.CompletionDate).IsRequired(false);
            builder.Property(er => er.Duration).IsRequired(false);
            builder.Property(er => er.TotalQuestions).IsRequired();
            builder.Property(er => er.CorrectAnswers).IsRequired(false);
            builder.Property(er => er.IncorrectAnswers).IsRequired(false);
            builder.Property(er => er.EmptyAnswers).IsRequired(false);

            builder.HasOne(er => er.User)
                .WithMany()
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(er => er.Exam)
                .WithMany()
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
