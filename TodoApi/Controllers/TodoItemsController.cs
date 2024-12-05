using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists/{todoListId}/todoitems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTodoItem([FromRoute]long todoListId, CreateTodoItem item)
        {
            var newItem = new TodoItem()
            {
                Description = item.Description,
                TodoListId = todoListId 
            };

            _context.TodoItems.Add(newItem);
             await _context.SaveChangesAsync();

            return CreatedAtAction("CreateTodoItem", newItem);
        }
    }
}
