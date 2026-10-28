using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi;

namespace TodoApi.Tests;

public class TodoApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Post_then_get_returns_created_todo()
    {
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/todos", new CreateTodo("Write demo"));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Todo>();
        var fetched = await client.GetFromJsonAsync<Todo>($"/todos/{created!.Id}");
        Assert.Equal("Write demo", fetched!.Title);
    }

    [Fact]
    public async Task Post_with_empty_title_returns_bad_request()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/todos", new CreateTodo(" "));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_unknown_todo_returns_not_found()
    {
        var response = await factory.CreateClient().DeleteAsync("/todos/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
