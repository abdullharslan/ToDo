using Entity.Concrete;

namespace Entity.DTO;

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public User User { get; set; }

    public LoginResponseDTO(string token, DateTime expiresAt, User user)
    {
        Token = token;
        ExpiresAt = expiresAt;
        User = user;
    }
}