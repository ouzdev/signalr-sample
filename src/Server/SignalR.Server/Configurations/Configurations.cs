using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SignalR.Server.Services;
using SignalR.Server.Services.Jwt;
using SignalR.Server.Services.SignalR;
using SignalR.Server.Settings;

namespace SignalR.Server.Configurations;

public static class Configurations
{
    /// <summary>
    /// Add Json Web Token Authentication
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication().AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKeys = new List<SecurityKey>
                {
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                }
            };
            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Headers["Authorization"];

                    if (!string.IsNullOrEmpty(accessToken) && context.Request.Path.StartsWithSegments("/notify-tracker"))
                    {
                        context.Token = accessToken.ToString().Replace("Bearer ", "");
                    }

                    return Task.CompletedTask;
                }
            };
        });
        
        services.AddScoped<IJwtService,JwtService>();
    }

    /// <summary>
    ///  Add Environment Variables to Configure Services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CoreSettings>(configuration.GetSection("Core"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    }

    /// <summary>
    /// Add SignalR Service
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static void AddSignalRService(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<ISignalRService, SignalRService>();
    }
}