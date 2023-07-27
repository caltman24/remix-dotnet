using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using TodoMe.Api;
using TodoMe.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.Authority = "https://unbiased-wahoo-97.clerk.accounts.dev";
        opts.Audience = "https://api.todome.com";

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier,
            ValidIssuer = "https://unbiased-wahoo-97.clerk.accounts.dev",
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization(opts =>
{
    opts.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("name")
        .RequireClaim(ClaimTypes.NameIdentifier)
        .Build();
});

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITodoRepository, TodoRepository>();
builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(pb =>
        pb.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .Build()
    );
});

var app = builder.Build();

app.MapTodoGroup()
    .MapUserGroup();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.Run();