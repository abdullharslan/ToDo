using Entity.Concrete;

namespace Business.Abstract;

/*
 * IAuthService arayüzü, kullanıcı kimlik doğrulama ve yetkilendirme işlemlerini yönetmek için gerekli
 * yöntemleri tanımlar. JWT (JSON Web Token) oluşturma, kullanıcı kaydı ve girişi gibi işlevler içerir.
 * Uygulama, bu arayüzü implement eden bir sınıf aracılığıyla kullanıcıların güvenli bir şekilde
 * kimlik doğrulaması yapmasını sağlar.
 */
public interface IAuthService
{
    // Kullanıcıdan bir JWT oluşturur
    string GenerateToken(User user);
    // Yeni bir kullanıcı kaydı oluşturur
    User RegisterUser(User user, string password);
    // Kullanıcı adı ve şifre ile kullanıcı girişini doğrular.
    User Login(string username, string password);
    // Belirli bir kullanıcı adının daha önce kayıtlı olup olmadığını kontrol eder.
    bool UserExists(string username);
}