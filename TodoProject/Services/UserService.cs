using TodoProject.Models;
using TodoProject.Repositories;

namespace TodoProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Register(User user)
        {
            _logger.LogInformation("Registering user {Username}", user.Username);
            await _userRepository.AddUser(user);
        }

        public async Task<User> Login(string username, string password)
        {
            _logger.LogInformation("User {Username} attempting to log in", username);
            bool isValid = await _userRepository.ValidateUser(username, password);
            return isValid ? await _userRepository.GetUserByUsername(username) : null;
        }
    }


}
