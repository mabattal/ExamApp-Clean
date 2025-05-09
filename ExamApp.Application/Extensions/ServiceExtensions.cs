using System.Reflection;
using System.Text;
using ExamApp.Application.Authentication;
using ExamApp.Application.Contracts;
using ExamApp.Application.Features.Answers;
using ExamApp.Application.Features.ExamResults;
using ExamApp.Application.Features.Exams;
using ExamApp.Application.Features.Questions;
using ExamApp.Application.Features.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ExamApp.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //.net'in default olarak ModelStateInvalidFilter'ı eklemesini engelledik
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExamResultService, ExamResultService>();
            services.AddScoped(sp => new Lazy<IExamResultService>(() => sp.GetRequiredService<IExamResultService>()));
            services.AddHostedService<ExamExpirationBackgroundService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IDateTimeUtcConversionService, DateTimeUtcConversionService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            //JWT ayarlarını ekledik
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization();


            return services;
        }
    }
}
