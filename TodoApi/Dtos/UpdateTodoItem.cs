namespace TodoApi.Dtos;

public class UpdateTodoItem
{
    public required long Id { get; set; }
    public required string Description { get; set; }
}

