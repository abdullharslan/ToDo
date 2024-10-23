using Entity.Abstract;

namespace Entity.Concrete;

// User sınıfı, IUser arayüzünü uygulayarak kullanıcı nesnelerini temsil eder.
public class User : EntityBase
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();

    public User()
    {
        ToDoItems = new List<ToDoItem>();
    }
    
    // Constructor : Kullanıcı oluşturulurken gerekli bilgileri alır ve atanır.
    public User(string username, string password, string firstName, string lastName)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Role = "User";
        CreatedAt = DateTime.Now;
    }

    public User(string username, string password, string role)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
    }
}