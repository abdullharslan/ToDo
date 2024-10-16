using Entity.Abstract;

namespace Entity.Concrete;

public class ToDoItem : ITodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }

    public ToDoItem(string title, string description, int userId)
    {
        Title = title;
        Description = description;
        UserId = userId;
        CreatedDate = DateTime.Now;
        IsCompleted = false;
    }
}