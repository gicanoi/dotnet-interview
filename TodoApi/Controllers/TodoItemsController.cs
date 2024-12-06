using Microsoft.AspNetCore.Mvc;
using TodoApi.Dtos;
using TodoApi.Data;

namespace TodoApi.Controllers;


[Route("api/todolists/{todoListId}/todoitems")]
[ApiController]
public class TodoItemsController : ControllerBase
{

    readonly ITodoItemDataService _dataService;

    public TodoItemsController(ITodoItemDataService dataService) => _dataService = dataService;

    [HttpPost]
    public async Task<ActionResult> CreateTodoItem([FromRoute] long todoListId, CreateTodoItem item)
    {
        var newItem = await _dataService.InsertTodoItem(todoListId, item);
        return base.CreatedAtAction("CreateTodoItem", newItem);
    }


    [HttpGet]
    public async Task<ActionResult> ListTodoItems([FromRoute] long todoListId)
    {
        var todoList = await _dataService.FindTodoList(todoListId);

        if (todoList == null)
            return NotFound();

        var items = await _dataService.GetTodoItems(todoListId);
        return Ok(items);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTodoItem([FromRoute] long todoListId, UpdateTodoItem updateTodoItem)
    {
        var todoList = await _dataService.FindTodoList(todoListId);

        if (todoList == null)
            return NotFound();

        var existingItem = await _dataService.FindTodoItem(updateTodoItem.Id);
        if (existingItem == null)
            return NotFound();

        await _dataService.UpdateTodoItem(updateTodoItem, existingItem);

        return Ok(existingItem);
    }

    [HttpPut("{itemId}/complete")]
    public async Task<ActionResult> CompleteTodoItem([FromRoute] long todoListId, [FromRoute] long itemId)
    {
        var todoList = await _dataService.FindTodoList(todoListId);

        if (todoList == null)
            return NotFound();

        var todoItem = await _dataService.FindTodoItem(itemId);
        if (todoItem == null)
            return NotFound();

        if (todoItem.Completed)
            return BadRequest("This item is already completed");

        await _dataService.CompleteTodoItem(todoItem);
        return Ok(todoItem);
    }


    [HttpDelete("{itemId}")]
    public async Task<ActionResult> DeleteTodoItem([FromRoute] long todoListId, [FromRoute] long itemId)
    {
        var todoList = await _dataService.FindTodoList(todoListId);

        if (todoList == null)
            return NotFound();

        var todoItem = await _dataService.FindTodoItem(itemId);
        if (todoItem == null)
            return NotFound();

        await _dataService.DeleteTodoItem(todoItem);

        return Ok(todoItem);
    }
}
