using Entity.Abstract;

namespace Entity.Concrete;

public class User : IUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /*
     * Constructor
     * Kullanıcı nesnelerini oluştururken gerekli bilgileri dışarıdan almak istediğim için oluşturdum. 
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