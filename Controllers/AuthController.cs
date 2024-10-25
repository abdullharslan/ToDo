using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entity.Concrete;
using Entity.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace Controllers
{
    /*
     * AuthController, kullanıcı kimlik doğrulama işlemleri için gerekli HTTP isteklerini yöneten bir denetleyicidir.
     * Kullanıcı kayıt, giriş ve JWT token oluşturma işlevlerini barındırır.
     */
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                _authService.RegisterUser(registerDto);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Kullanıcı girişi için endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = _authService.Login(loginDto);
                var token = _authService.GenerateToken(user);
                return Ok(new { Token = token, User = user });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message); // Kullanıcı bulunamadı
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message); // Geçersiz şifre
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Kullanıcı adı veya şifre boş
            }
        }

        // Kullanıcı var mı kontrolü için endpoint
        [HttpGet("exists/{username}")]
        public IActionResult UserExists(string username)
        {
            var exists = _authService.UserExists(username);
            return Ok(new { Exists = exists });
        }
    }
}