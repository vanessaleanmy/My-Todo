using System.ComponentModel.DataAnnotations;

namespace TodoListAppApi.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        [Required]
        public string Item { get; set; } = default!;
        public bool IsDone { get; set; }
    }
}
