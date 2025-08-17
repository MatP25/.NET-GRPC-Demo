public class Repository : IRepository
{

    private readonly ILogger<Repository> _logger;
    private readonly List<User> _users = new List<User>();
    public Repository(ILogger<Repository> logger)
    {
        _logger = logger;
        // Initialize with some sample data
        _users.Add(new User(1, "Pepe", "pepe@mail.com"));
        _users.Add(new User(2, "Popo", "popo@mail.com"));
        _users.Add(new User(3, "Pipi", "pipi@mail.com"));
    }

    public User AddUser(User user)
    {
        int newId = _users.Last().Id + 1;
        user.Id = newId;
        _users.Add(user);
        return _users.Last();
    }

    public List<User> GetUsers(int offset, int amount)
    {
        if (offset < 0 || amount < 0)
        {
            return _users;
        }
        return _users.Skip(offset).Take(amount).ToList();
    }

    public User? GetUserById(int id)
    {
        return _users.Find(u => u.Id == id);
    }

    public int DeleteUser(int id)
    {
        var user = GetUserById(id);
        if (user == null)
        {
            throw new Exception($"User with id {id} not found.");
        }
        _users.Remove(user);
        return id;
    }

    public ToDo AddToDo(int userId, string task)
    {
        User? user = _users.Find(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception($"User with id {userId} not found.");
        }

        ToDo toDo = new ToDo(
            user.ToDos.LastOrDefault()?.Id + 1 ?? 1,
            task
        );
        user.AddToDo(toDo);
        foreach (var u in _users) {
            _logger.LogInformation($"> User: {u.Name}, ToDos: {string.Join(", ", u.ToDos.Select(t => t.Task))}");
        }
        return toDo;
    }

    public void UpdateToDo(int userId, int toDoId, bool status)
    {
        User? user = _users.Find(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception($"User with id {userId} not found.");
        }
        ToDo? toDo = user.ToDos.Find(t => t.Id == toDoId);
        if (toDo == null)
        {
            throw new Exception($"ToDo with id {toDoId} not found for user {userId}.");
        }
        _logger.LogInformation($"Updating ToDo with id {toDoId} for user {userId} to status {status}.");
        toDo.IsCompleted = status;
    }
}