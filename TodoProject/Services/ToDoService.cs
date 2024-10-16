using TodoProject.Models;
using TodoProject.Repositories;

namespace TodoProject.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<ToDoService> _logger;

        public ToDoService(IToDoRepository toDoRepository, ILogger<ToDoService> logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ToDo>> GetToDos(int userId)
        {
            _logger.LogInformation("Fetching to-dos for user {UserId}", userId);
            return await _toDoRepository.GetToDosByUserId(userId);
        }

        public async Task<ToDo> GetToDoById(int toDoId)
        {
            _logger.LogInformation("Fetching to-do with id {ToDoId}", toDoId);
            return await _toDoRepository.GetToDoById(toDoId);
        }

        public async Task CreateToDo(ToDo toDo)
        {
            _logger.LogInformation("Creating new to-do with subject {Subject}", toDo.Subject);
            await _toDoRepository.AddToDo(toDo);
        }

        public async Task UpdateToDo(ToDo toDo)
        {
            _logger.LogInformation("Updating to-do with id {ToDoId}", toDo.ToDoId);
            await _toDoRepository.UpdateToDo(toDo);
        }

        public async Task DeleteToDo(int toDoId)
        {
            _logger.LogInformation("Deleting to-do with id {ToDoId}", toDoId);
            await _toDoRepository.DeleteToDo(toDoId);
        }

        public async Task MarkToDoStatus(int toDoId, string status)
        {
            _logger.LogInformation("Marking to-do {ToDoId} as {Status}", toDoId, status);
            var toDo = await _toDoRepository.GetToDoById(toDoId);
            if (toDo != null && toDo.Status != "Done" && toDo.Status != "Canceled")
            {
                toDo.Status = status;
                await _toDoRepository.UpdateToDo(toDo);
            }
        }
    }


}
