using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace TodoMe.Api.Extensions;

internal static class AuthExtensions
{
    public static AuthenticationBuilder AddClerkAuthentication(this IServiceCollection services, IConfiguration config)
    {
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.Authority = config["Authentication:Clerk:Authority"];
                opts.Audience = config["Authentication:Clerk:Audience"];

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidIssuer = config["Authentication:Clerk:Authority"],
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true
                };
            });
    }

    public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
    {
        return services.AddAuthorization(opts =>
        {
            opts.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.NameIdentifier)
                .Build();
        });
    }
}