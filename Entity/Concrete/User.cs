using Entity.Abstract;

namespace Entity.Concrete;

// User sınıfı, IUser arayüzünü uygulayarak kullanıcı nesnelerini temsil eder.
public class User : IUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Constructor : Kullanıcı oluşturulurken gerekli bilgileri alır ve atanır.
    public User(string username, string password)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        // Her kullanıcı User rolünde atandı.
        Role = "User";
        // Kullanıcı oluşturulduğunda zaman damgası ekle
        CreatedAt = DateTime.Now;
    }
}