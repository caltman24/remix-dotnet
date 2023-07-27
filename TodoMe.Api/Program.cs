using TodoMe.Api;
using TodoMe.Api.Extensions;
using TodoMe.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClerkAuthentication(builder.Configuration);
builder.Services.AddAuthorizationService();

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