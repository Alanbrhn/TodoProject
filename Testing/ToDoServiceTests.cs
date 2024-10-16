using Microsoft.Extensions.Logging;
using Moq;
using TodoProject.Models;
using TodoProject.Repositories;
using TodoProject.Services;


namespace Testing
{
    public class ToDoServiceTests
    {
        private readonly IToDoService _toDoService;
        private readonly Mock<IToDoRepository> _mockToDoRepository;

        public ToDoServiceTests()
        {
            _mockToDoRepository = new Mock<IToDoRepository>();
            _toDoService = new ToDoService(_mockToDoRepository.Object, Mock.Of<ILogger<ToDoService>>());
        }

        [Fact]
        public async Task CreateToDo_Should_Add_ToDo()
        {
            var newToDo = new ToDo { Subject = "Test Task", Description = "Test Description" };

            _mockToDoRepository.Setup(repo => repo.AddToDo(newToDo))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _toDoService.CreateToDo(newToDo);

            _mockToDoRepository.Verify(repo => repo.AddToDo(It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async Task GetToDos_Should_Return_ToDos_For_User()
        {
            var userId = 1;
            var toDos = new List<ToDo>
        {
            new ToDo { ToDoId = 1, Subject = "Task 1", UserId = 1 },
            new ToDo { ToDoId = 2, Subject = "Task 2", UserId = 1 }
        };

            _mockToDoRepository.Setup(repo => repo.GetToDosByUserId(userId))
                .ReturnsAsync(toDos);

            var result = await _toDoService.GetToDos(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task MarkToDoStatus_Should_Update_Status()
        {
            var toDoId = 1;
            var toDo = new ToDo { ToDoId = toDoId, Status = "Unmarked" };

            _mockToDoRepository.Setup(repo => repo.GetToDoById(toDoId))
                .ReturnsAsync(toDo);

            await _toDoService.MarkToDoStatus(toDoId, "Done");

            Assert.Equal("Done", toDo.Status);
            _mockToDoRepository.Verify(repo => repo.UpdateToDo(It.IsAny<ToDo>()), Times.Once);
        }

        [Fact]
        public async Task DeleteToDo_Should_Remove_ToDo()
        {
            var toDoId = 1;
            var toDo = new ToDo { ToDoId = toDoId, Status = "Unmarked" };

            _mockToDoRepository.Setup(repo => repo.GetToDoById(toDoId))
                .ReturnsAsync(toDo);
            _mockToDoRepository.Setup(repo => repo.DeleteToDo(toDoId))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _toDoService.DeleteToDo(toDoId);

            _mockToDoRepository.Verify(repo => repo.DeleteToDo(toDoId), Times.Once);
        }
    }
}