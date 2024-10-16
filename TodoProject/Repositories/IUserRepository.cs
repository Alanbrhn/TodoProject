using TodoProject.Models;

namespace TodoProject.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task AddUser(User user);
        Task<bool> ValidateUser(string username, string password);
    }


}
