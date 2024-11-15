using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoListAppApi.Controllers;
using TodoListAppApi.Models;
using TodoListAppApi.Services;

namespace TodoListAppTest
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoServices> _mockTodoService;
        private readonly TodoController _todoController;

        public TodoControllerTests()
        {
            _mockTodoService = new Mock<ITodoServices>();
            _todoController = new TodoController(_mockTodoService.Object);
        }

        [Fact]
        public async void GetTodoList_ShouldReturnOkWithTodoList()
        {
            // Arrange
            var todoList = new List<Todo>
            {
                new Todo { Item = "First Item", IsDone = false },
                new Todo { Item = "Second Item", IsDone = false }
            };
            _mockTodoService.Setup(service => service.GetAll()).ReturnsAsync(todoList);

            // Act
            var result = await _todoController.GetTodoList();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(todoList);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedResultWithTodo()
        {
            // Arrange
            var newTodo = new Todo { Item = "New Todo", IsDone = false };
            var createdTodo = new Todo { Id = Guid.NewGuid(), Item = "New Todo", IsDone = false };
            _mockTodoService.Setup(service => service.AddTodo(newTodo)).ReturnsAsync(createdTodo);

            // Act
            var result = await _todoController.Create(newTodo);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(createdTodo);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenTodoExists()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            _mockTodoService.Setup(service => service.DeleteTodo(todoId)).ReturnsAsync(true);

            // Act
            var result = await _todoController.DeleteTodo(todoId);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenTodoDoesNotExist()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            _mockTodoService.Setup(service => service.DeleteTodo(todoId)).ReturnsAsync(false);

            // Act
            var result = await _todoController.DeleteTodo(todoId);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenTodoExists()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            _mockTodoService.Setup(service => service.UpdateTodoStatus(todoId, true)).ReturnsAsync(true);

            // Act
            var result = await _todoController.UpdateTodoStatus(todoId,true);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenTodoDoesNotExist()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            _mockTodoService.Setup(service => service.UpdateTodoStatus(todoId, true)).ReturnsAsync(false);

            // Act
            var result = await _todoController.UpdateTodoStatus(todoId, true);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }
    }
}