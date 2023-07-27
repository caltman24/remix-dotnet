using Microsoft.AspNetCore.Mvc;
using TodoMe.Api.Entities;
using TodoMe.Api.Repositories;

namespace TodoMe.Api;

internal static class TodoModule
{
    public static IEndpointRouteBuilder MapTodoGroup(this IEndpointRouteBuilder app)
    {
        var todoGroup = app.MapGroup("/todos")
            .RequireAuthorization();

        // Get All
        todoGroup.MapGet("/", async (ITodoRepository todoRepository, HttpContext ctx) =>
        {
            var userId = GetUserNameIdentifier(ctx);
            var todos = await todoRepository.GetAllAsync(userId);

            return Results.Ok(todos);
        });

        // Get By id
        todoGroup.MapGet("/{id:int}", async (
            int id,
            ITodoRepository todoRepository,
            HttpContext ctx) =>
        {
            var userId = GetUserNameIdentifier(ctx);
            var todo = await todoRepository.GetByIdAsync(id, userId);

            return todo is null ? Results.NotFound() : Results.Ok(todo);
        }).WithName("GetTodoById");

        // New Todo
        todoGroup.MapPost("/", async (
            ITodoRepository todoRepository,
            HttpContext ctx,
            [FromBody] Todo todo) =>
        {
            var userId = GetUserNameIdentifier(ctx);
            await todoRepository.AddAsync(todo, userId);

            return Results.CreatedAtRoute("GetTodoById", new { id = todo.Id }, todo);
        });

        // Delete Todo
        todoGroup.MapDelete("/{id:int}", async (
            ITodoRepository todoRepository,
            HttpContext ctx,
            int id) =>
        {
            var userId = GetUserNameIdentifier(ctx);
            await todoRepository.DeleteAsync(id, userId);

            return Results.NoContent();
        });

        return app;
    }

    private static UserId GetUserNameIdentifier(HttpContext context)
    {
        if (context.User.Identity?.Name is null)
        {
            throw new NullReferenceException("No claims identity found");
        }

        return context.User.Identity.Name;
    }
}