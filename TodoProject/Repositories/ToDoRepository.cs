using Microsoft.EntityFrameworkCore;
using TodoProject.DataContext;
using TodoProject.Models;

namespace TodoProject.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _context;

        public ToDoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToDo(ToDo toDo)
        {
            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDo(int toDoId)
        {
            var toDo = await GetToDoById(toDoId);
            if (toDo != null && toDo.Status == "Unmarked")
            {
                _context.ToDos.Remove(toDo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ToDo>> GetToDosByUserId(int userId)
        {
            return await _context.ToDos.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<ToDo> GetToDoById(int toDoId)
        {
            return await _context.ToDos.FindAsync(toDoId);
        }

        public async Task UpdateToDo(ToDo toDo)
        {

            var existingToDo = await _context.ToDos.FindAsync(toDo.ToDoId);
            if (existingToDo == null)
            {
                throw new InvalidOperationException("ToDo not found.");
            }

            existingToDo.Subject = toDo.Subject;
            existingToDo.Description = toDo.Description;
            existingToDo.Status = toDo.Status;

            await _context.SaveChangesAsync();
        }
    }


}
