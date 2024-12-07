using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Models;

using TodoApi.Data;
using TodoApi.Dtos;

namespace TodoApi.Tests;

#nullable disable
public class TodoItemsControllerTests
{
    private DbContextOptions<TodoContext> DatabaseContextOptions()
    {
        return new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    private void PopulateDatabaseContext(TodoContext context)
    {
        context.TodoList.Add(new TodoList { Id = 1, Name = "Task 1" });
        context.TodoList.Add(new TodoList { Id = 2, Name = "Task 2" });
        context.TodoItems.Add(new TodoItem { Id = 1, Description = "Do something", TodoListId = 1 });
        context.TodoItems.Add(new TodoItem { Id = 2, Description = "Do something else", TodoListId = 1 });
        context.TodoItems.Add(new TodoItem { Id = 3, Description = "Do another thing", TodoListId = 1 });
        context.TodoItems.Add(new TodoItem { Id = 4, Description = "THIS IS ALREADY DONE!", TodoListId = 1, Completed = true });
        context.SaveChanges();
    }

    [Fact]
    public async Task GetTodoItems_WhenCalled_ReturnsTodoItemsList()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));

        var result = await controller.ListTodoItems(1);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(4, ((result as OkObjectResult).Value as IList<TodoItem>).Count);

    }

    [Fact]
    public async Task GetTodoItems_WhenCalledWithNonExistingListId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));

        var result = await controller.ListTodoItems(100000);

        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async Task CreateTodoItem_WhenCalled_ReturnsOkResult()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var aDescription = "new item";
        var result = await controller.CreateTodoItem(1, new Dtos.CreateTodoItem() { Description = aDescription });

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(aDescription, ((result as CreatedAtActionResult).Value as TodoItem).Description);
    }

    [Fact]
    public async Task CreateTodoItem_WhenCalledWithNonExistingListId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var aDescription = "new description";
        var result = await controller.CreateTodoItem(1000000, new Dtos.CreateTodoItem() { Description = aDescription });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateTodoItem_WhenCalledWithNonExistingListId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var aDescription = "new description";
        var result = await controller.UpdateTodoItem(10000, 2, new Dtos.UpdateTodoItem() { Description = aDescription });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateTodoItem_WhenCalled_ReturnsOk()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var aDescription = "new description";
        var result = await controller.UpdateTodoItem(1, 1, new Dtos.UpdateTodoItem() { Description = aDescription });

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(aDescription, ((result as OkObjectResult).Value as TodoItem).Description);

    }

    [Fact]
    public async Task UpdateTodoItem_WhenCalledWithNonExistingItemId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var aDescription = "new item";
        var result = await controller.UpdateTodoItem(1, 10000000, new Dtos.UpdateTodoItem() { Description = aDescription });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CompleteTodoItem_WhenCalled_ReturnsOK()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.CompleteTodoItem(1, 1);

        Assert.IsType<OkObjectResult>(result);
        Assert.True(((result as OkObjectResult).Value as TodoItem).Completed);
    }

    [Fact]
    public async Task CompleteTodoItem_WhenCalledWithNonExistingItemId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.CompleteTodoItem(1, 10000000);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CompleteTodoItem_WhenCalledWithAlreadyCompletedItemId_ReturnsBadRequest()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.CompleteTodoItem(1, 4);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(Errors.ITEM_ALREADY_COMPLETED, (result as BadRequestObjectResult).Value as string);

    }


    [Fact]
    public async Task CompleteTodoItem_WhenCalledWithNonExistingTodoListId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.CompleteTodoItem(1000000, 1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteTodoItem_WhenCalled_ReturnsOK()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.DeleteTodoItem(1, 1);

        Assert.IsType<OkObjectResult>(result);

    }

    [Fact]
    public async Task DeleteTodoItem_WhenCalledWithNonExistingItemId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.CompleteTodoItem(1, 10000000);

        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async Task DeleteTodoItem_WhenCalledWithNonExistingTodoListId_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseContext(context);

        var controller = new TodoItemsController(CreateDataService(context));
        var result = await controller.CompleteTodoItem(1000000, 1);

        Assert.IsType<NotFoundResult>(result);
    }




    private static TodoItemDataService CreateDataService(TodoContext context) => new TodoItemDataService(context);
}
