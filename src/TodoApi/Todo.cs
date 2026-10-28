namespace TodoApi;

public record Todo(int Id, string Title, bool Done = false);

public record CreateTodo(string Title);

public class TodoStore
{
    private readonly List<Todo> _todos = [];
    private int _nextId = 1;

    public IReadOnlyList<Todo> All()
    {
        lock (_todos) return _todos.ToList();
    }

    public Todo? Get(int id)
    {
        lock (_todos) return _todos.FirstOrDefault(t => t.Id == id);
    }

    public Todo Add(string title)
    {
        lock (_todos)
        {
            var todo = new Todo(_nextId++, title.Trim());
            _todos.Add(todo);
            return todo;
        }
    }

    public bool Remove(int id)
    {
        lock (_todos) return _todos.RemoveAll(t => t.Id == id) > 0;
    }
}
