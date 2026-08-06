using AIResume.Application.Users.DTOs;
using AIResume.Application.Users.Interfaces;
using AIResume.Application.Users.Repositories;
using AIResume.Application.Users.Services;
using AIResume.Application.Users.Validators;
using AIResume.Infrastructure.Persistence;
using AIResume.Infrastructure.Repositories;
using AIResume.Infrastructure.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<
    IUserRegistrationService,
    UserRegistrationService>();

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IPasswordHasherService,
    PasswordHasherService>();

builder.Services.AddScoped<
    IValidator<RegisterUserRequest>,
    RegisterUserRequestValidator>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();