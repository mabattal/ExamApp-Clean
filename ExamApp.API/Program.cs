using ExamApp.API.Extensions;
using ExamApp.Application.Extensions;
using ExamApp.Authentication.Extensions;
using ExamApp.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddCustomControllersWithFilters()
    .AddCustomSwagger()
    .AddCustomExceptionHandler()
    .AddCustomCaching()
    .AddCustomCors();

//Repository'leri, Service'leri ve jwt ayarlarýný ekledik(Extension olarak)
builder.Services
    .AddRepositories(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddAuthenticationServices(builder.Configuration);

var app = builder.Build();

app.UseCustomPipeline();

app.MapControllers();

app.Run();
