using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.Authentication.Extensions;
using Trippie.Common.Services.Authentication.Jwt;
using Trippie.Common.Services.Authentication.Security;

namespace Trippie.Common.Services.Authentication.DependencyInjection;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddTrippieAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                  ?? throw new InvalidOperationException($"Missing configuration section '{JwtOptions.SectionName}'.");

        var keyBytes = Encoding.UTF8.GetBytes(jwt.SecretKey);

        if (keyBytes.Length < 32)
            throw new InvalidOperationException("Jwt:Secretkey must be at least 32 bytes (256 bits) for HS256");

        var signingKey = new SymmetricSecurityKey(keyBytes);

        services.AddMemoryCache();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<ITokenBlocklistService, MemoryCacheTokenBlocklistService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.MapInboundClaims = false;
                options.SaveToken = false;
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.FromSeconds(jwt.ClockSkewSeconds),
                    NameClaimType = JwtClaimTypes.Username,
                    RoleClaimType = "role"
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        if (ctx.Exception is SecurityTokenExpiredException)
                            ctx.Response.Headers.Append("Token-Expired", "true");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        var blocklist = ctx.HttpContext.RequestServices
                            .GetRequiredService<ITokenBlocklistService>();
                        var jti = ctx.Principal?.GetJti();
                        if (jti is not null && blocklist.IsRevoked(jti))
                            ctx.Fail("Token has been revoked.");
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}