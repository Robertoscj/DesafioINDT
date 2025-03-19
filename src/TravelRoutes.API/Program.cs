using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TravelRoutes.Application.Mappings;
using TravelRoutes.Domain.Interfaces;
using TravelRoutes.Infrastructure.Repositories;
using TravelRoutes.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using TravelRoutes.Application.Validators;
using TravelRoutes.Application.DTOs;
using TravelRoutes.API.Middleware;
using System.Reflection;
using System.IO;
using Swashbuckle.AspNetCore.Filters;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<RouteValidator>();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Travel Routes API", 
        Version = "v1",
        Description = "API para gerenciamento de rotas de viagem e busca do melhor preço",
        Contact = new OpenApiContact
        {
            Name = "INDT",
            Email = "contato@indt.org.br"
        }
    });

   
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    
    c.ExampleFilters();

    // Add operation filter to add security requirements
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new string[] {}
        }
    });
});


builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Validators
builder.Services.AddScoped<IValidator<RouteDto>, RouteValidator>();
builder.Services.AddScoped<IValidator<CreateRouteDto>, CreateRouteValidator>();
builder.Services.AddScoped<IValidator<RouteResponseDto>, RouteResponseValidator>();

// Add Services
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IRouteRepository>(sp => 
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    return new RouteRepository(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel Routes API V1");
        c.RoutePrefix = string.Empty;
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}


app.UseInputValidation();


app.UseIpRateLimiting();


app.UseErrorHandling();

app.UseAuthorization();
app.MapControllers();

app.Run();
