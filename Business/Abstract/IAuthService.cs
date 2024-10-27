using Entity.Concrete;
using Entity.DTO;

namespace Business.Abstract;

/*
 * IAuthService arayüzü, kullanıcı kimlik doğrulama ve yetkilendirme işlemlerini yönetmek için gerekli
 * yöntemleri tanımlar. JWT (JSON Web Token) oluşturma, kullanıcı kaydı ve girişi gibi işlevler içerir.
 * Uygulama, bu arayüzü implement eden bir sınıf aracılığıyla kullanıcıların güvenli bir şekilde
 * kimlik doğrulaması yapmasını sağlar.
 */
public interface IAuthService
{
    string GenerateToken(User user);
    Task<LoginResponseDTO> RegisterUserAsync(RegisterDTO registerDto);
    Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
    Task<bool> UserExistsAsync(string username);
}