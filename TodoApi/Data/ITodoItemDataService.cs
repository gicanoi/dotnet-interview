using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Data;

public interface ITodoItemDataService
{
    Task DeleteTodoItem(TodoItem todoItem);
    Task<TodoItem> InsertTodoItem(long todoListId, CreateTodoItem item);
    Task<TodoList?> FindTodoList(long todoListId);
    Task<TodoItem?> FindTodoItem(long itemId);
    Task UpdateTodoItem(UpdateTodoItem newState, TodoItem existingItem);
    Task CompleteTodoItem(TodoItem existingItem);
    Task<IEnumerable<TodoItem>> GetTodoItems(long todoListId);
}
