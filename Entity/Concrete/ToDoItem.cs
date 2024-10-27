using Entity.Abstract;

namespace Entity.Concrete;

// ToDoItem sınıfı, ITodoItem arayüzünü uygulayarak görev nesnelerini temsil eder.
public sealed class ToDoItem : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    // Default constructor
    public ToDoItem()
    {
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
    }

    // Parametreli constructor
    public ToDoItem(string title, string description, int userId)
    {
        Title = title;
        Description = description;
        UserId = userId;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
}