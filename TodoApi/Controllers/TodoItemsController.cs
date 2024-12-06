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
        public async Task<ActionResult> CreateTodoItem([FromRoute] long todoListId, CreateTodoItem item)
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

        [HttpPut]
        public async Task<ActionResult> UpdateTodoItem([FromRoute] long todoListId, UpdateTodoItem item)
        {
            var todoList = await _context.TodoList.FindAsync(todoListId);

            if (todoList == null)
                return NotFound();

            var todoItem = await _context.TodoItems.FindAsync(item.Id);
            if (todoItem == null)
                return NotFound();

            todoItem.Description = item.Description;
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }

        [HttpPut("{itemId}/complete")]
        public async Task<ActionResult> CompleteTodoItem([FromRoute] long todoListId, [FromRoute] long itemId)
        {
            var todoList = await _context.TodoList.FindAsync(todoListId);

            if (todoList == null)
                return NotFound();

            var todoItem = await _context.TodoItems.FindAsync(itemId);
            if (todoItem == null)
                return NotFound();

            if (todoItem.Completed)
                return BadRequest("This item is already completed");

            todoItem.Completed = true;
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }


        [HttpDelete("{itemId}")]
        public async Task<ActionResult> DeleteTodoItem([FromRoute] long todoListId, [FromRoute] long itemId)
        {
            var todoList = await _context.TodoList.FindAsync(todoListId);

            if (todoList == null)
                return NotFound();

            var todoItem = await _context.TodoItems.FindAsync(itemId);
            if (todoItem == null)
                return NotFound();

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }
    }
}
