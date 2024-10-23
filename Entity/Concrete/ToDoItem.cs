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

    // Constructor: Görev oluşturulurken gerekli bilgileri alır ve atanır.
    public ToDoItem(string title, string description, int userId, User user)
    {
        Title = string.IsNullOrWhiteSpace(title) ? 
            throw new ArgumentException("Başlık boş bırakılamaz.", nameof(title)) : title;
        Description = string.IsNullOrWhiteSpace(description) ? 
            throw new ArgumentException("Açıklama boş bırakılamaz.", nameof(description)) : description;
        UserId = userId;
        User = user;
        CreatedAt = DateTime.Now;
        IsCompleted = false;
        
    }
}