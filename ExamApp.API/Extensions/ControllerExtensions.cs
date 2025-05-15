using ExamApp.API.Filters;

namespace ExamApp.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddCustomControllersWithFilters(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<FluentValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
            return services;
        }
    }
}
