namespace ExamApp.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}
