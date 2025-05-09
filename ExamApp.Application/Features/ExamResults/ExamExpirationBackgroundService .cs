using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExamApp.Application.Features.ExamResults
{
    public class ExamExpirationBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ExamExpirationBackgroundService> logger) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var examResultService = scope.ServiceProvider.GetRequiredService<IExamResultService>();

                    logger.LogInformation("Checking for expired exams...");
                    await examResultService.AutoSubmitExpiredExamsAsync();

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // 1 dakikada bir kontrol et
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while processing expired exams.");
                }
            }
        }
    }
}
