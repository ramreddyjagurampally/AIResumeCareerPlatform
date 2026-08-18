using AIResume.Application.Jobs.Interfaces;
using AIResume.Application.Jobs.Services;
using AIResume.Application.Resumes.Interfaces;
using AIResume.Application.Resumes.Repositories;
using AIResume.Application.Resumes.Services;
using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;
using AIResume.Application.Users.Repositories;
using AIResume.Application.Users.Services;
using AIResume.Application.Users.Validators;
using AIResume.Infrastructure.Parsing;
using AIResume.Infrastructure.Persistence;
using AIResume.Infrastructure.Repositories;
using AIResume.Infrastructure.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Application services
builder.Services.AddScoped<
    IUserRegistrationService,
    UserRegistrationService>();

builder.Services.AddScoped<
    IUserLoginService,
    UserLoginService>();

builder.Services.AddScoped<
    IResumeUploadService,
    ResumeUploadService>();

builder.Services.AddScoped<
    IResumeParserService,
    PdfResumeParserService>();

builder.Services.AddScoped<
    IResumeAnalysisService,
    ResumeAnalysisService>();

builder.Services.AddScoped<
    IJobMatchService,
    JobMatchService>();

// Repositories
builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IResumeRepository,
    ResumeRepository>();

// Security services
builder.Services.AddScoped<
    IPasswordHasherService,
    PasswordHasherService>();

builder.Services.AddScoped<
    IJwtTokenService,
    JwtTokenService>();

// Validation
builder.Services.AddScoped<
    IValidator<RegisterUserRequest>,
    RegisterUserRequestValidator>();

// Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://nice-dune-038ee5e10.7.azurestaticapps.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// JWT Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(
                    "JWT ERROR: " +
                    context.Exception.Message);

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();