using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PicPay.Api.Configs;

public static class AuthenticationConfigs
{
    public const string BearerScheme = "Bearer";

    public static void AddAuthenticationConfigs(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetService<AuthSettings>()!;

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(settings.SecurityKey)
            ),

            ValidAlgorithms = [ "HS256" ],

            ValidateAudience = true,
            ValidAudience = settings.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            RoleClaimType = "role",
        };

        services.AddAuthentication(BearerScheme)
            .AddJwtBearer(BearerScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
    }
}
