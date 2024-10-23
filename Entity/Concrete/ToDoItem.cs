using Entity.Abstract;

namespace Entity.Concrete;

// ToDoItem sınıfı, ITodoItem arayüzünü uygulayarak görev nesnelerini temsil eder.
public sealed class ToDoItem : EntityBase
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}