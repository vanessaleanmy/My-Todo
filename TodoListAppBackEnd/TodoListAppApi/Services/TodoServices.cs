using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoListAppApi.Data;
using TodoListAppApi.Models;

namespace TodoListAppApi.Services
{
    public interface ITodoServices {
        Task<List<Todo>> GetAll();
        Task<Todo> AddTodo(Todo newTodo);
        Task<bool> DeleteTodo(Guid id);
        Task<bool> UpdateTodoStatus(Guid id, bool completed);
    }

    public class TodoServices: ITodoServices
    {
        private readonly TodoDbContext _todoDbContext;
        public TodoServices(TodoDbContext todoDbContext) {
            _todoDbContext = todoDbContext;
        }

        public async Task<List<Todo>> GetAll() { 
            var todoList = await _todoDbContext.Todos.ToListAsync();
            return todoList;
        }

        public async Task<Todo> AddTodo(Todo newTodo)
        {
            if (string.IsNullOrWhiteSpace(newTodo.Item))
            {
                throw new ArgumentException("Todo item is required");
            }
            Todo newItem = new Todo { Id = Guid.NewGuid(), Item = newTodo.Item, IsDone = false };
            await _todoDbContext.Todos.AddAsync(newItem);
            await _todoDbContext.SaveChangesAsync();
            return newItem;
        }

        public async Task<bool> DeleteTodo(Guid id) {
            var todo = await _todoDbContext.Todos.FindAsync(id);
            if (todo == null) { return false; }
            _todoDbContext.Todos.Remove(todo);
            await _todoDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTodoStatus(Guid id, bool completed)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);
            if (todo == null) return false;

            todo.IsDone = completed;
            await _todoDbContext.SaveChangesAsync();
            return true;
        }
    }
}
