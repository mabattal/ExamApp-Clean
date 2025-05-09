using ExamApp.Domain.Entities.Common;
using ExamApp.Domain.Enums;

namespace ExamApp.Domain.Entities
{
    public class User : IAuditEntity
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
