using Microsoft.Extensions.Logging;
using Moq;
using TodoProject.Models;
using TodoProject.Repositories;
using TodoProject.Services;

namespace Testing
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepository;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object, Mock.Of<ILogger<UserService>>());
        }

        [Fact]
        public async Task Register_Should_Add_New_User()
        {
            var newUser = new User { Username = "testuser", Password = "password123" };

            _mockUserRepository.Setup(repo => repo.AddUser(newUser))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _userService.Register(newUser);

            _mockUserRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Login_Should_Return_User_When_Valid()
        {
            var username = "testuser";
            var password = "password123";
            var user = new User { UserId = 1, Username = "testuser", Password = "password123" };

            _mockUserRepository.Setup(repo => repo.ValidateUser(username, password))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username))
                .ReturnsAsync(user);

            var result = await _userService.Login(username, password);

            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task Login_Should_Return_Null_When_Invalid()
        {
            var username = "invaliduser";
            var password = "wrongpassword";

            _mockUserRepository.Setup(repo => repo.ValidateUser(username, password))
                .ReturnsAsync(false);

            var result = await _userService.Login(username, password);

            Assert.Null(result);
        }
    }
}
