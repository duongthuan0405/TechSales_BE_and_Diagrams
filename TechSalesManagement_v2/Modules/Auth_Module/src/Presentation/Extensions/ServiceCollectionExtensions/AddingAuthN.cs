
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auth_Module.src.Presentation.Extensions.ServiceCollectionExtensions;

public static partial class  ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthN(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["AUTH:JWT:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not found in configuration.");
        var issuer = configuration["AUTH:JWT:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found in configuration.");
        var audience = configuration["AUTH:JWT:Audience"] ?? throw new InvalidOperationException("JWT Audience not found in configuration.");
        
        var keyBytes = Convert.FromBase64String(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; 
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ClockSkew = TimeSpan.Zero
            };
            
        });

    
        return services;
    }
}
