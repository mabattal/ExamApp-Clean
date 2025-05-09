namespace ExamApp.Application.Contracts
{
    public interface IDateTimeUtcConversionService
    {
        DateTimeOffset ConvertToUtc(DateTimeOffset localDateTime);
        DateTimeOffset ConvertFromUtc(DateTimeOffset utcDateTime);
    }
}
