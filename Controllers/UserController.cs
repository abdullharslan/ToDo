using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

/*
 * UserController, kullanıcı yönetimi ile ilgili işlemleri yöneten bir API denetleyicisidir. Bu sınıf, kullanıcı ekleme,
 * güncelleme, silme ve kullanıcı bilgilerini alma işlemlerini gerçekleştirir. İş mantığı, IUserService arayüzü
 * üzerinden sağlanır.
 */

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    /*
     * IUserService arayüzünü uygulayan bir nesne, Dependency Injection yöntemiyle UserController'a aktarılır. Bu
     * sayede, iş mantığı ile ilgili işlemler (kullanıcı ekleme vb.) _userService üzerinden gerçekleştirilir. 'readonly'
     * anahtar kelimesi, _userService değişkeninin yalnızca yapıcı metod içinde atanabileceğini ve sonrasında
     * değiştirilemeyeceğini belirtir. Bu, kodun güvenilirliğini artırır ve kullanıcı işlemleri için gereken hizmetin
     * tutarlı bir şekilde kullanılmasını sağlar.
     */
    // Dependecy Constructor Injection
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:int}")]
    public IActionResult GetUser(int id)
    {
        /*
         * Belirtilen kullanıcı kimliğine sahip kullanıcıyı alır. Kullanıcı bulunamazsa, InvalidOperationException
         * fırlatılır.
         */
        var user = _userService.GetUser(id) ?? throw new InvalidOperationException("Kullanici Bulunamadı");
        return Ok(user);
    }

    [HttpPost]
    public IActionResult Add([FromBody] User user)
    {
        // Kullanıcı nesnesinin null olup olmadığını kontrol eder ve bir istisna fırlatır.
        _ = user ?? throw new ArgumentNullException(nameof(user), "Kullanıcı verileri eksik");
        
        try
        {
            _userService.Add(user);
            /*
             * Kullanıcı başarıyla eklendiğinde, yeni kaynak için 201 Created döndür. CreatedAtAction metodu, eklenen
             * kullanıcının detaylarını döndürmek için GetUser metodunu kullanarak, oluşturulan kullanıcının ID'sini içerir.
             */
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            /*
             * Eğer bir InvalidOperationException hatası oluşursa, bu durumda 409 Conflict döndür ve "Kullanıcı
             * bulunamadı veya mevcut" mesajını ekle.
             */
            return Conflict("Kullanici bulunamadı veya mevcut : " + ex.Message);
        }
        catch (Exception ex)
        {
            // Genel hata durumları için
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] User user)
    {
        // Kullanıcı verilerini kontrol et, eğer user null ise, ArgumentNullException fırlat
        _ = user ?? throw new ArgumentNullException(nameof(user), "Kullanıcı verileri eksik"); 
        
        /*
         * Kullanıcı kimliğinin eşleşip eşleşmediğini kontrol et, eşleşmiyorsa, InvalidOperationException fırlat.
         */
        if (id != user.Id)
        {
            return BadRequest("Kullanıcı veriler eksik");
        }

        try
        {
            // Kullanıcıyı güncelle
            _userService.Update(user);
            // Güncelleme başarılıysa 204 No Content gönder.
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Kullanıcı bulunamadı veya başka bir sorun var
            return NotFound("Kullanici bulunamdı: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Genel hata durumları için
            return StatusCode(500, "Sunucu hatası" + ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            // Kullanıcıyı al ve null ise istisna fırlat
            var user = _userService.GetUser(id) ?? throw new InvalidOperationException("Kullanici bulunamdı.");
            // Kullanıcıyı sil
            _userService.Delete(user);
            // 204 No Content döndür
            return NoContent();

        }
        catch (InvalidOperationException ex)
        {
            // Kullanıcı bulunamadıysa 404 döndür
            return NotFound("İşlem yapılamadı" + ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Sunucu hatası: " + ex.Message);
        }
    }
}