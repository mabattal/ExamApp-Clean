using ExamApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamApp.Persistence.Questions
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.QuestionId);
            builder.Property(q => q.QuestionText).IsRequired().HasMaxLength(1000);
            builder.Property(q => q.OptionA).IsRequired().HasMaxLength(200);
            builder.Property(q => q.OptionB).IsRequired().HasMaxLength(200);
            builder.Property(q => q.OptionC).IsRequired().HasMaxLength(200);
            builder.Property(q => q.OptionD).IsRequired().HasMaxLength(200);
            builder.Property(q => q.CorrectAnswer).IsRequired().HasMaxLength(200);

            builder.HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
