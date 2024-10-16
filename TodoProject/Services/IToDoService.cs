using TodoProject.Models;

namespace TodoProject.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDo>> GetToDos(int userId);
        Task<ToDo> GetToDoById(int toDoId);
        Task CreateToDo(ToDo toDo);
        Task UpdateToDo(ToDo toDo);
        Task DeleteToDo(int toDoId);
        Task MarkToDoStatus(int toDoId, string status);
    }



}
