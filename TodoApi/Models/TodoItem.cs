namespace TodoApi.Models;

public class TodoItem 
{
    
    public long Id {get;set;}
    public long TodoListId {get;set;}
    public string Description {get;set;}
    public bool Completed {get;set;}

}



