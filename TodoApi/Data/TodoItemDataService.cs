using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Data;

public class TodoItemDataService : ITodoItemDataService
{

    private readonly TodoContext _context;

    public TodoItemDataService(TodoContext context) => _context = context;

    public async Task DeleteTodoItem(TodoItem todoItem)
    {
        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
    }

    public async Task<TodoItem> InsertTodoItem(long todoListId, CreateTodoItem item)
    {
        var newItem = new TodoItem()
        {
            Description = item.Description,
            TodoListId = todoListId
        };

        _context.TodoItems.Add(newItem);
        await _context.SaveChangesAsync();
        return newItem;
    }

    public async Task<TodoList?> FindTodoList(long todoListId) => await _context.TodoList.FindAsync(todoListId);

    public async Task<TodoItem?> FindTodoItem(long itemId) => await _context.TodoItems.FindAsync(itemId);

    public async Task UpdateTodoItem(UpdateTodoItem newState, TodoItem existingItem)
    {
        existingItem.Description = newState.Description;
        await _context.SaveChangesAsync();
    }

    public async Task CompleteTodoItem(TodoItem existingItem)
    {
        existingItem.Completed = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetTodoItems(long todoListId) 
        => await _context.TodoItems.Where(item => item.TodoListId == todoListId).ToListAsync();
}
