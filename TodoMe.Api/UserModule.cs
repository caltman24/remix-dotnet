using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TodoMe.Api.Entities;
using TodoMe.Api.Repositories;

namespace TodoMe.Api;

internal static class UserModule
{
    public static IEndpointRouteBuilder MapUserGroup(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/users").RequireAuthorization();

        userGroup.MapGet("/", async (IUserRepository userRepository) =>
            Results.Ok(await userRepository.GetAllAsync()));

        userGroup.MapGet("/{id}", async (IUserRepository userRepository, string id) =>
        {
            var user = await userRepository.GetByIdAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        }).WithName("GetUserById");

        userGroup.MapPost("/", async (IUserRepository userRepository, HttpContext ctx) =>
        {
            var user = new User()
            {
                Id = ctx.User.Identity!.Name!,
                Name = ctx.User.Claims.First(x => x.Type == "name").Value
            };
            
            if (await userRepository.AddAsync(user))
            {
                return Results.CreatedAtRoute("GetUserById", new { id = user.Id }, user);
            }

            return Results.Conflict($"User {user.Id} already exists");
        });

        return app;
    }
}