namespace Entity.Concrete;

// LoginRequest sınıfı, kullanıcı giriş bilgilerini temsil eder.
public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}