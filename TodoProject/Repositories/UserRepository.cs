using Microsoft.EntityFrameworkCore;
using TodoProject.DataContext;
using TodoProject.Models;

namespace TodoProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ValidateUser(string username, string password)
        {
            var user = await GetUserByUsername(username);
            return user != null && user.Password == password;
        }
    }


}
