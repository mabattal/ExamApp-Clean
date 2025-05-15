using ExamApp.API.ExceptionHandlers;
using ExamApp.API.Extensions;
using ExamApp.API.Filters;
using ExamApp.Application.Contracts.Caching;
using ExamApp.Application.Extensions;
using ExamApp.Authentication.Extensions;
using ExamApp.Caching;
using ExamApp.Persistence.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Swagger'ý jwt ile kullanabilmek için düzenledik
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExamApp API", Version = "v1" });

    // JWT desteðini ekliyoruz
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "To access the API using JWT, enter the token in the following format: \n\n **Bearer {token}**"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

//Repository'leri, Service'leri ve jwt ayarlarýný ekledik(Extension olarak)
builder.Services
    .AddRepositories(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddAuthenticationServices(builder.Configuration);

builder.Services.AddExceptionHandler<CriticalExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

//Cors ayarlarýný ekledik. Bu ayarý yapma sebebimiz, client tarafýnda farklý bir domainde çalýþan uygulamalarýn bu apiye eriþebilmesi için
builder.Services.AddCustomCors(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(x => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.SeedDatabase();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomCors();

app.MapControllers();

app.Run();
