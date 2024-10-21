using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Concrete;

/*
 * AuthService sınıfı, IAuthService arayüzünü implement ederek kullanıcı kimlik doğrulama işlemlerini yönetir.
 * Bu sınıf, kullanıcı kayıt ve giriş işlemleri için gerekli olan yöntemleri sağlar. Ayrıca, kullanıcıdan alınan
 * bilgileri kullanarak JWT (JSON Web Token) oluşturur ve doğrular. AuthService, IUserRepository arayüzünü kullanarak
 * kullanıcı verileri ile etkileşimde bulunur ve uygulamanın güvenliğini artırmak için yapılandırma dosyasındaki
 * gizli anahtarı kullanır. Sınıf, kullanıcı adı ve şifre kontrolü yaparak, kullanıcıların güvenli bir şekilde
 * sisteme giriş yapmasını sağlar ve yetkilendirme sürecinde JWT'yi yönetir.
 */
public class AuthService : IAuthService
{
    /*
     * AuthService sınıfı, kullanıcı kimlik doğrulama ve JWT (JSON Web Token) yönetimi ile ilgili iş mantığını içerir.
     * Bu sınıf, kullanıcı kayıt, giriş ve token oluşturma işlemlerini gerçekleştirmek için IUserRepository arayüzünü
     * kullanarak veritabanı ile etkileşimde bulunur. Sınıfın içinde ayrıca, JWT oluşturmak için gerekli olan gizli
     * anahtar (_secretKey) saklanır. Bu anahtar, JWT'nin güvenliğini sağlamak ve yetkilendirme süreçlerinde
     * kullanılmak üzere IConfiguration üzerinden yapılandırma dosyasından alınır.
     */
    private readonly IUserRepository _userRepository;
    // JWT için gizli anahtar
    private readonly string? _secretKey;

    public AuthService(IUserRepository userRepository, string key, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _secretKey = configuration["Jwt:Key"];
    }

    // JWT token oluşturur
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.Now.AddDays(1); // Token 1 gün geçerli

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public User RegisterUser(User user, string password)
    {
        // Kullanıcının kullanıcı adı ve şifresinin boş olup olmadığını kontrol et.
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException("Kullanıcı adı veya şifre boş olamaz.");
        }

        // Kullanıcının daha önce kayıtlı olup olmadığını kontrol et.
        if (_userRepository.GetByUsername(user.Username) != null)
        {
            throw new InvalidOperationException("Kullanıcı adı zaten mevcut.");
        }

        // Kullanıcı nesnesinin parolasını hash'lenmeden direkt atanır ve kaydet.
        user.Password = password;
        _userRepository.Add(user);

        return user;
    }

    public User Login(string username, string password)
    {
        // Kullanıcı adı ve şifresinin boş olup olmadığını kontrol et.
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException("Kullanıcı adı veya şifre boş olamaz.");
        }

        // Kullanıcıyı kullanıcı adı ile bul.
        var user = _userRepository.GetByUsername(username);
        if (user == null)
        {
            throw new InvalidOperationException("Kullanıcı bulunamadı.");
        }

        // Kullanıcının parolasını doğrula. (Hash'leme yoksa, basit bir kontrol yapılıyor)
        if (user.Password != password)
        {
            throw new UnauthorizedAccessException("Geçersiz şifre.");
        }

        // Kullanıcı başarılı bir şekilde giriş yaptıysa, kullanıcıyı döndür.
        return user; 
    }
    
    public bool UserExists(string username)
    {
        // Kullanıcı adı boşsa false döndür
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }

        // Kullanıcıyı kullanıcı adı ile bul
        var user = _userRepository.GetByUsername(username);

        // Eğer kullanıcı varsa true, yoksa false döndür
        return user != null;
    }
}