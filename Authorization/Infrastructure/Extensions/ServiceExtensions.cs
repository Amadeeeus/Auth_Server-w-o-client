using System.Reflection;
using System.Text;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.JWT;
using Authorization.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Infrastructure.Extensions;
public static class ServiceExtensions
{
    
    public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserEntity>());
        return services;
    }

    public static void AddApiAuthentification(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            var SecretKey = Environment.GetEnvironmentVariable("SECRETKEY");
            options.TokenValidationParameters = new()
            {
        
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
            };
        });
    }

    public static void AddStackExchangeRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis:Configuration").Value;
            options.InstanceName = configuration.GetSection("Redis:InstanceName").Value;
        });
    }

    public static void AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(opt => opt.AddPolicy("DefaultPolicy",policy =>
        {
            policy.WithOrigins("http://localhost:4200");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        }));
    }

    public static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
    
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped(typeof(IDistributedCacheRepository<>), typeof(DistributedCacheRepository<>));

    }
}