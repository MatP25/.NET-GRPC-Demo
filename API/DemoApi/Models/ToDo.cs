
public class ToDo
{
    public int Id { get; set; }
    public string Task { get; set; }
    public bool IsCompleted { get; set; }

    public ToDo(int id, string task)
    {
        Id = id;
        Task = task;
        IsCompleted = false;
    }
}
