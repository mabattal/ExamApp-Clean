using ExamApp.Authentication.Extensions;
using ExamApp.Persistence.Extensions;

namespace ExamApp.API.Extensions
{
    public static class ConfigurePipelineExtensions
    {
        public static IApplicationBuilder UseCustomPipeline(this WebApplication app)
        {
            app.UseCustomExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCustomSwaggerExt();
            }

            app.UseCustomSeedDatabase();

            app.UseHttpsRedirection();

            app.UseCustomAuthenticationServices();

            app.UseCustomCors();
            return app;
        }
    }
}
