using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.Business.Abstractions.Services;
using NotificationService.Business.Exceptions;
using NotificationService.Business.Kafka;
using NotificationService.Business.Services;
using NotificationService.Repository;
using NotificationService.Shared.DTO.Request;
using OperationService.Api.Validators;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpClient("AuthApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthApiClient:BaseAddress"]!);
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add configuration for DB
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Kafka
builder.Services.AddHostedService<KafkaNotificationConsumerService>();

// Add Services
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Register validators
builder.Services.AddScoped<IValidator<SendMessageRequest>, SendMessageRequestValidator>();


builder.Services.AddCors(options =>
{
    // Default policy 
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = $"http://{httpReq.Host.Value}/notification-api" }
        });
    });
    app.UseSwaggerUI();
}

// Global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();