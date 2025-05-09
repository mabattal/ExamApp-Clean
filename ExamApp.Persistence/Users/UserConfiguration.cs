using ExamApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamApp.Persistence.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();
            //builder.HasQueryFilter(u => !u.IsDeleted);
            builder.Property(u => u.FullName).HasMaxLength(100);
        }
    }
}
