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
    
    /*
     * Constructor : Kullanıcı oluşturulurken gerekli bilgileri alır ve atanır.
     */
    public User(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
        // Kullanıcı oluşturulduğunda zaman damgası ekle
        CreatedAt = DateTime.Now;
    }
}