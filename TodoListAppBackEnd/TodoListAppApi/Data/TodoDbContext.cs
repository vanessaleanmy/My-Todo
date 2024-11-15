using Microsoft.EntityFrameworkCore;
using TodoListAppApi.Models;

namespace TodoListAppApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public DbSet<Todo> Todos { get; set; }
    }
}
