namespace ExamApp.Application.Contracts.Authentication
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string role);
    }
}
