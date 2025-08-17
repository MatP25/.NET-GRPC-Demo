public interface IRepository
{
    User AddUser(User user);
    List<User> GetUsers(int offset, int amount);
    User? GetUserById(int id);
    int DeleteUser(int id);
    ToDo AddToDo(int userId, string task);

    void UpdateToDo(int userId, int toDoId, bool status);
}
