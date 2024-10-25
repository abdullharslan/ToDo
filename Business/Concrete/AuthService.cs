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

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _secretKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Key is not configured.");
            _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Audience is not configured.");
            _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured.");
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddDays(1); 

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void RegisterUser (RegisterDTO registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentNullException("Kullanıcı adı veya şifre boş olamaz.");
            }

            if (_userRepository.GetByUsername(registerDto.Username) != null)
            {
                throw new InvalidOperationException("Kullanıcı adı zaten mevcut.");
            }
            
            _userRepository.Add(new User
            {
                Username = registerDto.Username,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Password = registerDto.Password
            });
        }

        public User Login(LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                throw new ArgumentNullException("Kullanıcı adı veya şifre boş olamaz.");
            }

            var user = _userRepository.GetByUsername(loginDto.Username);
            if (user == null || user.Password != loginDto.Password) // Düz metin karşılaştırması
            {
                throw new UnauthorizedAccessException("Geçersiz kullanıcı adı veya şifre.");
            }

            return user;
        }

        public bool UserExists(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            return _userRepository.GetByUsername(username) != null;
        }
    }
}