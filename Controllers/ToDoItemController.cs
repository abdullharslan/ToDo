using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

/*
 * TodoItemController, görev yönetimi ile ilgili işlemleri yöneten bir API denetleyicisidir. Bu sınıf, görev ekleme,
 * güncelleme, silme ve görev bilgilerini alma işlemlerini gerçekleştirir. İş mantığı, ITodoItemService arayüzü
 * üzerinden sağlanır.
 */
namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemController : Controller
{
    /*
     * ITodoItemService arayüzünü uygulayan bir nesne, Dependency Injection yöntemiyle TodoItemController'a aktarılır.
     * Bu sayede, görev yönetimi ile ilgili işlemler (görev ekleme, güncelleme, silme vb.) _todoItemService üzerinden
     * gerçekleştirilir. 'readonly' anahtar kelimesi, _todoItemService değişkeninin yalnızca yapıcı metod içinde
     * atanabileceğini ve sonrasında değiştirilemeyeceğini belirtir. Bu, kodun güvenilirliğini artırır ve görev
     * işlemleri için gereken hizmetin tutarlı bir şekilde kullanılmasını sağlar.
     */
    // Dependecy Constructor Injection
    private readonly IToDoItemService _toDoItemService;

    public ToDoItemController(IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;
    }

    [HttpGet("{id:int}")]
    public IActionResult GetTodoItem(int id)
    {
        /*
         * Belirtilen kimliğe sahip görevi almak için hizmetten istek yapar. Eğer görev bulunamazsa,
         * InvalidOperationException fırlatılarak kullanıcıya görev bulunamadığına dair bir hata mesajı iletilir.
         * Başarılı bir durumda, görev bilgileri 200 OK durumu ile döndürülür.
         */
        var todoItem = _toDoItemService.GetTodoItem(id);
        if (todoItem == null) return NotFound("Görev bulunamadı.");
        return Ok(todoItem);
    }

    [HttpPost]
    public IActionResult Add([FromBody] ToDoItem todoItem)
    {
        if (todoItem == null) return BadRequest("Görev verileri eksik.");
        
        try
        {
            _toDoItemService.Add(todoItem);
            /*
             * Görev başarıyla eklendiğinde, 201 Created durumu döndürür ve oluşturulan görevin detaylarını döndürmek
             * için GetTodoItem metodunu kullanır.
             */
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }
        catch (Exception ex)
        {
            // Genel bir hata durumu meydana gelirse, 500 Internal Server Error döndürü ve hata mesajını içerir.
            return StatusCode(500, "Sunucu hatası: " + ex);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] ToDoItem todoItem)
    {
        if (todoItem == null) return BadRequest("Görev verileri eksik.");
        
        // Gelen id ile todoItem'ın kimliğini karşılaştır; eşleşmiyorsa BadRequest döndür.
        if (id != todoItem.Id)
        {
            return BadRequest("Görev kimliği eşleşmiyor.");
        }

        try
        {
            // TodoItem'ı güncelleme işlemini gerçekleştir.
            _toDoItemService.Update(todoItem);
            // Güncelleme başarılıysa 204 No Content döndür.
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Eğer bir InvalidOperationException hatası oluşursa, 404 Not Found döndür ve hata mesajını ekle.
            return NotFound("Görev bulunamadı" + ex.Message);
        }

        catch (Exception ex)
        {
            // Genel hata durumları için 500 Internal Server Error döndür ve hata mesajını içerir.
            return StatusCode(500, "Sunucu hatası: " + ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var todoItem = _toDoItemService.GetTodoItem(id);
        if (todoItem == null) return NotFound("Görev bulunamadı.");
        
        try
        {
            _toDoItemService.Delete(todoItem);
            // Silme işlemi başarılıysa 204 No Content döndür.
            return NoContent();
        }
        catch (Exception ex)
        {
            // Genel hata durumları için 500 Internal Server Error döndür ve hata mesajını içerir.
            return StatusCode(500, "Sunucu hatası: " + ex.Message);
        }
    }
}