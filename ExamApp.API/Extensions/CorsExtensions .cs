namespace ExamApp.API.Extensions
{
    //tüm domainlerden gelen istekleri kabul etmesi için
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
        {
            return app.UseCors("AllowAll");
        }
    }

    //belirlenen domainlerdeki uygulamaların bu apiye erişebilmesi için
    //public static class CorsExtensions
    //{
    //    private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    //    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    //    {
    //        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

    //        services.AddCors(options =>
    //        {
    //            options.AddPolicy(MyAllowSpecificOrigins, policy =>
    //            {
    //                policy.WithOrigins(allowedOrigins!)
    //                    .AllowAnyMethod()
    //                    .AllowAnyHeader();
    //            });
    //        });

    //        return services;
    //    }

    //    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    //    {
    //        return app.UseCors(MyAllowSpecificOrigins);
    //    }
    //}

}
