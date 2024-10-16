using Entity.Abstract;

namespace Entity.Concrete;

// ToDoItem sınıfı, ITodoItem arayüzünü uygulayarak görev nesnelerini temsil eder.
public class ToDoItem : ITodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }

    // Constructor: Görev oluşturulurken gerekli bilgileri alır ve atanır.
    public ToDoItem(string title, string description, int userId)
    {
        Title = title;
        Description = description;
        UserId = userId;
        CreatedDate = DateTime.Now;
        IsCompleted = false;
    }
}