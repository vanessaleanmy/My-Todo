using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TodoListAppApi.Data;
using TodoListAppApi.Models;
using TodoListAppApi.Services;

namespace TodoListAppTest
{
    public class TodoServiceTests
    {
        private TodoDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoTestDbService")
                .Options;
            return new TodoDbContext(options);
        }

        [Fact]
        public async void AddTodo_ValidTodoModel_ShouldAddSuccessfully()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var todoService = new TodoServices(dbContext);
            var todoToAdd = new Todo { Item = "Buy Bread", IsDone = false };

            // Act
            var result = await todoService.AddTodo(todoToAdd);

            // Assert
            result.Should().NotBeNull();
            result.Item.Should().Be("Buy Bread");
            result.IsDone.Should().BeFalse();
        }

        [Fact]
        public async void AddTodo_WithoutItem_ShouldThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var todoService = new TodoServices(dbContext);
            var todoToAdd = new Todo { Item = "", IsDone = false };

            // Act & Assert
            Func<Task> act = async () => await todoService.AddTodo(todoToAdd);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Todo item is required");
        }

        [Fact]
        public async Task GetAllTodo_WhenDataExist_ShouldReturnAllTodo()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var todoService = new TodoServices(dbContext);
            var addTodoList = new List<Todo> {
                new Todo { Item = "First Item", IsDone = false },
                new Todo { Item = "Second Item", IsDone = false }
            };
            dbContext.AddRange(addTodoList);
            await dbContext.SaveChangesAsync();

            // Act
            var todoList = await todoService.GetAll();

            // Assert
            todoList.Should().HaveCount(2);
            todoList.Select(t => t.Item).Should().Contain(new[] { "First Item", "Second Item" });
        }

        [Fact]
        public async Task DeleteTodo_ShouldRemoveTodoItem()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var todoService = new TodoServices(dbContext);
            var addTodoList = new List<Todo> {
                new Todo { Item = "Delete Me", IsDone = false },
                new Todo { Item = "Second Item", IsDone = false }
            };
            dbContext.AddRange(addTodoList);
            await dbContext.SaveChangesAsync();
            var todoToDelete = await dbContext.Todos.FirstOrDefaultAsync(td => td.Item == "Delete Me");

            // Act
            var result = await todoService.DeleteTodo(todoToDelete!.Id);

            // Assert
            result.Should().BeTrue();
            var deletedTodo = await dbContext.Todos.FindAsync(todoToDelete.Id);
            deletedTodo.Should().BeNull();
        }

        [Fact]
        public async Task UpdateTodo_WithStatusNotDone_ShouldUpdateToDone()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var todoService = new TodoServices(dbContext);
            var addTodoList = new List<Todo> {
                new Todo { Item = "Update Me", IsDone = false },
                new Todo { Item = "Second Item", IsDone = false }
            };
            dbContext.AddRange(addTodoList);
            await dbContext.SaveChangesAsync();
            var todoToUpdateDone = await dbContext.Todos.FirstOrDefaultAsync(td => td.Item == "Update Me");

            // Act
            var result = await todoService.UpdateTodoStatus(todoToUpdateDone!.Id,true);

            // Assert
            result.Should().BeTrue();
            var updatedTodo = await dbContext.Todos.FindAsync(todoToUpdateDone.Id);
            updatedTodo.Should().NotBeNull();
            updatedTodo.IsDone.Should().BeTrue();
        }
    }
}
