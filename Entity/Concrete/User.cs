using Entity.Abstract;

namespace Entity.Concrete;

// User sınıfı, IUser arayüzünü uygulayarak kullanıcı nesnelerini temsil eder.
public class User : EntityBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // Navigation property - ToDoItems ile ilişkisi
    public ICollection<ToDoItem> ToDoItems { get; set; }

    // Default constructor
    public User()
    {
        ToDoItems = new List<ToDoItem>();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
    
    // Parametreli constructor
    public User(string username, string password, string firstName, string lastName)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        ToDoItems = new List<ToDoItem>();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
}