using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entity.Concrete;
using Entity.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace Controllers
{
    /*
     * AuthController, kullanıcı kimlik doğrulama ve yetkilendirme işlemleri için HTTP endpoint'leri sağlar.
     * Bu controller:
     * - Yeni kullanıcı kaydı (Register)
     * - Kullanıcı girişi (Login)
     * - Kullanıcı kontrolü (UserExists)
     * işlemlerini yönetir ve JWT token tabanlı kimlik doğrulama sistemi kullanır.
     */
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /*
         * _authService: Kimlik doğrulama işlemleri için kullanılan servis
         * Constructor Dependency Injection ile IAuthService enjekte edilir
         */
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        /*
         * Yeni kullanıcı kaydı oluşturur.
         * [HttpPost] - POST metodu ile çağrılır
         * [FromBody] - RegisterDTO verisi request body'den alınır
         *
         * Dönüş Değerleri:
         * 200 OK - Başarılı kayıt durumunda JWT token ve kullanıcı bilgileri
         * 400 Bad Request - Geçersiz veri durumunda
         * 409 Conflict - Kullanıcı adı zaten mevcutsa
         */
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var responce = await _authService.RegisterUserAsync(registerDto);
                return Ok(responce);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /*
         * Kullanıcı girişi yapar ve JWT token oluşturur.
         * [HttpPost] - POST metodu ile çağrılır
         * [FromBody] - LoginDTO verisi request body'den alınır
         *
         * Dönüş Değerleri:
         * 200 OK - Başarılı giriş durumunda JWT token ve kullanıcı bilgileri
         * 400 Bad Request - Geçersiz veri durumunda
         * 401 Unauthorized - Geçersiz kullanıcı adı veya şifre
         */
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
               var responce = await _authService.LoginAsync(loginDto);
               return Ok(responce);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /*
         * Kullanıcı adının sistemde var olup olmadığını kontrol eder.
         * [HttpGet] - GET metodu ile çağrılır
         * {username} - URL'den kullanıcı adı parametresi alınır
         *
         * Dönüş Değerleri:
         * 200 OK - Kontrol sonucu (true/false)
         * 400 Bad Request - Geçersiz kullanıcı adı
         */
        [HttpGet("exists/{username}")]
        public async Task<IActionResult> UserExists(string username)
        {
            try
            {
                var exists = await _authService.UserExistsAsync(username);
                return Ok(new { Exists = exists });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}