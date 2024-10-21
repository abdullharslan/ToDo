using Entity.Abstract;

namespace Entity.Concrete;

// ToDoItem sınıfı, ITodoItem arayüzünü uygulayarak görev nesnelerini temsil eder.
public class ToDoItem : ITodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }

    // Constructor: Görev oluşturulurken gerekli bilgileri alır ve atanır.
    public ToDoItem(string title, string description, int userId)
    {
        Title = string.IsNullOrWhiteSpace(title) ? 
            throw new ArgumentException("Başlık boş bırakılamaz.", nameof(title)) : title;
        Description = string.IsNullOrWhiteSpace(description) ? 
            throw new ArgumentException("Açıklama boş bırakılamaz.", nameof(description)) : description;
        UserId = userId;
        CreatedAt = DateTime.Now;
        IsCompleted = false;
    }
}