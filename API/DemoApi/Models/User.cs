public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public List<ToDo> ToDos { get; set; } = [];

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
    public User(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void AddToDo(ToDo toDo)
    {
        ToDos.Add(toDo);
    }

    public void updateToDo(int id, bool status)
    {
        ToDo? toDo = ToDos.FirstOrDefault(t => t.Id == id);
        if (toDo != null)
        {
            toDo.IsCompleted = status;
        }
        else
        {
            throw new Exception("ToDo not found");
        }
    }


}