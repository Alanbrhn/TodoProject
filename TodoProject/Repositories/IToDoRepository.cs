using TodoProject.Models;

namespace TodoProject.Repositories
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDo>> GetToDosByUserId(int userId);
        Task<ToDo> GetToDoById(int toDoId);
        Task AddToDo(ToDo toDo);
        Task UpdateToDo(ToDo toDo);
        Task DeleteToDo(int toDoId);
    }


}
