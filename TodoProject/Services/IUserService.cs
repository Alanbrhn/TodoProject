using TodoProject.Models;

namespace TodoProject.Services
{
    public interface IUserService
    {
        Task Register(User user);
        Task<User> Login(string username, string password);
    }


}
