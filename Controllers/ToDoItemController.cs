using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

/*
 * ToDoItemController, kullanıcıların görev yönetimi için HTTP endpoint'leri sağlar.
 * Bu controller:
 * - Görev ekleme, güncelleme, silme işlemleri
 * - Görev detayı görüntüleme
 * - Tamamlanma durumuna göre görev filtreleme
 * işlemlerini yönetir ve kullanıcı bazlı yetkilendirme sistemi kullanır.
 */
[ApiController]
// Sadece giriş yapmış kullanıcılar erişebilir.
[Authorize]
[Route("api/[controller]")]

// Controller yerine ControllerBase kullanıyoruz çünkü view dönmeyeceğiz
public class ToDoItemController : Controller
{
    /*
     * _todoItemService: Görev işlemleri için kullanılan servis
     * Constructor Dependency Injection ile IToDoItemService enjekte edilir
     */
    private readonly IToDoItemService _toDoItemService;

    public ToDoItemController(IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;
    }

    /*
     * Belirtilen ID'ye sahip görevi getirir.
     * [HttpGet] - GET metodu ile çağrılır
     * {id} - URL'den görev ID'si alınır
     *
     * Dönüş Değerleri:
     * 200 OK - Görev başarıyla bulunduğunda
     * 404 Not Found - Görev bulunamadığında
     * 401 Unauthorized - Kullanıcı yetkisiz olduğunda
     */
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodoItem(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            var todoItem = await _toDoItemService.GetTodoItemAsync(id, userId);
            if (todoItem == null)
            {
                return NotFound(new { message = "Görev bulunamdı." });
            }

            return Ok(todoItem);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }

    /*
     * Kullanıcının görevlerini tamamlanma durumuna göre filtreli getirir.
     * [HttpGet] - GET metodu ile çağrılır
     * [FromQuery] isCompleted - Query string'den filtreleme parametresi alınır
     */
    [HttpGet]
    public async Task<IActionResult> GetFilteredItems([FromBody] bool? isCompleted = null)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            var items = await _toDoItemService.GetFilteredItemsAsync(userId, isCompleted);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }
    
    /*
     * Yeni görev ekler.
     * [HttpPost] - POST metodu ile çağrılır
     * [FromBody] - ToDoItem verisi request body'den alınır
     */
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            return BadRequest(new { message = "Görev verileri eksik." });
        }
        
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            todoItem.UserId = userId;

            await _toDoItemService.AddAsync(todoItem);
            if (await _toDoItemService.SaveChangesAsync())
            {
                return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
            }
            
            return BadRequest(new { message = "Görev kaydedilemedi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }

    /*
     * Mevcut görevi günceller.
     * [HttpPut] - PUT metodu ile çağrılır
     * {id} - URL'den görev ID'si alınır
     * [FromBody] - Güncellenmiş ToDoItem verisi request body'den alınır
     */
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            return BadRequest(new { message = "Görev verileri eksik." });
        }
        if (id != todoItem.Id)
        {
            return BadRequest(new { message = "Görev kimliği eşleşmiyor." });
        }

        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            todoItem.UserId = userId;

            await _toDoItemService.UpdateAsync(todoItem);
            if (await _toDoItemService.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest(new { message = "Görev güncellenemedi" });
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
     * Görevi siler.
     * [HttpDelete] - DELETE metodu ile çağrılır
     * {id} - URL'den görev ID'si alınır
     */
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
            var todoItem = await _toDoItemService.GetTodoItemAsync(id, userId);

            if (todoItem == null)
            {
                return NotFound(new { message = "Görev bulunamadı."});
            }

            await _toDoItemService.DeleteAsync(todoItem);
            if (await _toDoItemService.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest(new { message = "Görev silinemedi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Sunucu hatası: {ex.Message}" });
        }
    }
}