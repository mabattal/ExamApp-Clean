using ExamApp.Application.Contracts;
using ExamApp.Application.Features.Answers;
using ExamApp.Application.Features.ExamResults;
using ExamApp.Application.Features.Exams;
using ExamApp.Application.Features.Questions;
using ExamApp.Application.Features.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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


            return services;
        }
    }
}
