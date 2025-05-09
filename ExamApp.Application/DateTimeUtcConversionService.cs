using ExamApp.Application.Contracts;

namespace ExamApp.Application
{
    public class DateTimeUtcConversionService : IDateTimeUtcConversionService
    {
        private readonly TimeZoneInfo _turkeyTimeZone;

        public DateTimeUtcConversionService()
        {
            _turkeyTimeZone = GetTurkeyTimeZone();
        }

        public DateTimeOffset ConvertToUtc(DateTimeOffset localDateTime)
        {
            return localDateTime.ToUniversalTime();
        }

        public DateTimeOffset ConvertFromUtc(DateTimeOffset utcDateTime)
        {
            // UTC saatini yerel saate dönüştürme
            //return utcDateTime.ToLocalTime();
            // UTC'yi Türkiye saat dilimine göre dönüştür
            var turkeyTime = TimeZoneInfo.ConvertTime(utcDateTime, _turkeyTimeZone);
            return turkeyTime;
        }

        private TimeZoneInfo GetTurkeyTimeZone()
        {
            try
            {
                // Windows sistemlerde
                return TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    // Linux/macOS sistemlerde
                    return TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
                }
                catch (TimeZoneNotFoundException ex)
                {
                    throw new Exception("Turkey time zone not found on this system.", ex);
                }
            }
        }
    }
}