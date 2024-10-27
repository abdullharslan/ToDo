using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _secretKey, _audience, _issuer;
        private readonly int _tokenExpirationInHours;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _secretKey = configuration["Jwt:Key"] 
                         ?? throw new InvalidOperationException("Key is not configured.");
            _audience = configuration["Jwt:Audience"] 
                        ?? throw new InvalidOperationException("Audience is not configured.");
            _issuer = configuration["Jwt:Issuer"] 
                      ?? throw new InvalidOperationException("Issuer is not configured.");
            _tokenExpirationInHours = int.Parse(configuration["Jwt:ExpirationHours"] ?? "24");
        }

        /*
         * Kullanıcı bilgilerini kullanarak JWT token oluşturur. Token'a kullanıcı kimliği ve kullanıcı adı claim olarak
         * eklenir.Token'ın geçerlilik süresi _tokenExpirationInHours ile belirlenir.
         */
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddDays(_tokenExpirationInHours); 

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*
         * Yeni kullanıcı kaydı oluşturur. Kullanıcı adının benzersiz olduğunu kontrol eder. Şifreyi hash'leyerek
         * güvenli bir şekilde saklar.
         * Başarılı kayıt sonrası JWT token ve kullanıcı bilgilerini döner.
         */
        public async Task<LoginResponseDTO> RegisterUserAsync (RegisterDTO registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentNullException("Kullanıcı adı veya şifre boş olamaz.");
            }

            if (await UserExistsAsync(registerDto.Username))
            {
                throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor.");
            }
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Password = hashedPassword,
            };
            
            await _userRepository.AddAsync(user);
            if (!await _userRepository.SaveChangesAsync())
            {
                throw new InvalidOperationException("Kullanıcı kaydı yapılırken bir hata oluştu.");
            }
            
            var token = GenerateToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(_tokenExpirationInHours);
            
            return new LoginResponseDTO(token, expiresAt, user);
        }

        /*
         * Kullanıcı girişi yapar. Kullanıcı adı ve şifre doğruluğunu kontrol eder. Başarılı giriş sonrası JWT token ve
         * kullanıcı bilgilerini döner.
         */
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                throw new ArgumentNullException("Kullanıcı adı veya şifre boş olamaz.");
            }

            var user = await _userRepository.GetByUsername(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Geçersiz kullanıcı adı veya şifre.");
            }

            var token = GenerateToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(_tokenExpirationInHours);
            
            return new LoginResponseDTO(token, expiresAt, user);
        }

        /*
         * Belirtilen kullanıcı adının sistemde var olup olmadığını kontrol eder.
         */
        public async Task<bool> UserExistsAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "Kullanıcı adı boş olamaz.");
            }

            var user = await _userRepository.GetByUsername(username);
            return user != null;
        }
    }
}