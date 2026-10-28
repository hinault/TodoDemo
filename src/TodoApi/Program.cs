using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<TodoStore>();

var app = builder.Build();

app.MapGet("/todos", (TodoStore store) => store.All());

app.MapGet("/todos/{id:int}", (int id, TodoStore store) =>
    store.Get(id) is { } todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/todos", (CreateTodo request, TodoStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title is required.");

    var todo = store.Add(request.Title);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapDelete("/todos/{id:int}", (int id, TodoStore store) =>
    store.Remove(id) ? Results.NoContent() : Results.NotFound());

app.Run();

public partial class Program;
