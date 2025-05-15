using ExamApp.API.ExceptionHandlers;

namespace ExamApp.API.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static IServiceCollection AddCustomExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(x => { });
            return app;
        }
    }
}
