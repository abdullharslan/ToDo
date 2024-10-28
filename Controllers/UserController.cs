using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

/*
 * UserController, kullanıcı yönetimi için HTTP endpoint'leri sağlar.
 * Bu controller:
 * - Kullanıcı bilgisi görüntüleme
 * - Kullanıcı ekleme (Register işlemi AuthController'da)
 * - Kullanıcı bilgilerini güncelleme
 * - Kullanıcı silme (soft delete)
 * işlemlerini yönetir ve yetkilendirme sistemi kullanır.
 */
[ApiController]
// Sadece giriş yapmış kullanıcılar erişebilir
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    /*
     * _userService: Kullanıcı işlemleri için kullanılan servis
     * Constructor Dependency Injection ile IUserService enjekte edilir
     */
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /*
     * Belirtilen ID'ye sahip kullanıcıyı getirir.
     * [HttpGet] - GET metodu ile çağrılır
     * {id} - URL'den kullanıcı ID'si alınır
     *
     * Dönüş Değerleri:
     * 200 OK - Kullanıcı başarıyla bulunduğunda
     * 404 Not Found - Kullanıcı bulunamadığında
     * 401 Unauthorized - Yetkisiz erişim durumunda
     */
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var currentUserId = 
                int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            if (currentUserId != id)
            {
                return Unauthorized(new { message = "Başka kullanıcların bilgilerine erişemezsiniz." });
            }

            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }

    /*
     * Mevcut kullanıcının bilgilerini günceller.
     * [HttpPut] - PUT metodu ile çağrılır
     * {id} - URL'den kullanıcı ID'si alınır
     * [FromBody] - Güncellenmiş User verisi request body'den alınır
     */
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest(new { message = "Kullanıcı verileri eksik." });
        }

        try
        {
            var currentUserId = 
                int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            if (currentUserId != id || currentUserId != user.Id)
            {
                return Unauthorized(new { message = "Başka kullanıcıların bilgilerini güncelleyemezsin." });
            }

            await _userService.UpdateAsync(user);
            if (await _userService.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest(new { message = "Kullanıcı güncellenemedi." });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }
    
    /*
     * Kullanıcıyı siler (soft delete).
     * [HttpDelete] - DELETE metodu ile çağrılır
     * {id} - URL'den kullanıcı ID'si alınır
     */
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var currentUserId = 
                int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            if (currentUserId != id)
            {
                return Unauthorized(new { message = "Başka kullanıcıları silemezsin." });
            }
            
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            
            await _userService.DeleteAsync(user);
            if (await _userService.SaveChangesAsync())
            {
                return NoContent();
            }
            
            return BadRequest(new { message = "Kullanıcı silinemedi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }
}