using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoListAppApi.Data;
using TodoListAppApi.Models;
using TodoListAppApi.Services;

namespace TodoListAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoServices _todoServices;
        public TodoController(ITodoServices todoServices) {
            _todoServices = todoServices;
        }
        
        [ProducesResponseType(typeof(List<Todo>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetTodoList() {
            var result = await _todoServices.GetAll();
            return Ok(result);

        }

        [ProducesResponseType(typeof(Todo),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Todo newTodo)
        {            
            if (newTodo == null || string.IsNullOrWhiteSpace(newTodo.Item))
            {
                return BadRequest("Item is required");
            }
            var result = await _todoServices.AddTodo(newTodo);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]        
        public async Task<IActionResult> DeleteTodo(Guid id) {
            var deleted = await _todoServices.DeleteTodo(id);
            if (deleted) { 
                return NoContent();
            }
            return NotFound();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}/completed")]
        public async Task<IActionResult> UpdateTodoStatus(Guid id, [FromBody] bool completed)
        {
            var updated = await _todoServices.UpdateTodoStatus(id, completed);
            if (updated) return NoContent(); 
            return NotFound(); 
        }


    }
}
