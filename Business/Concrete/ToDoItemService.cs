using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete;

/*
 * TodoItemService sınıfı, ITodoItemService arayüzünü implement ederek görevlerle ilgili iş mantığını yönetir. Bu sınıf,
 * görevlerin eklenmesi, güncellenmesi, silinmesi ve görev bilgilerini alma işlemlerini gerçekleştirir. Ayrıca, iş
 * kurallarını uygulayarak veritabanı ile etkileşimde bulunur.
 */
public class ToDoItemService : IToDoItemService
{
    /*
     * TodoItemService sınıfının yapıcı metodu, dışarıdan bir ITodoItemRepository nesnesi alır ve bu nesneyi
     * _todoItemRepository değişkenine atar. Bu sayede sınıf içinde görev işlemleri için kullanılacak repository
     * nesnesine erişim sağlanır ve görevlerin eklenmesi, güncellenmesi, silinmesi gibi işlemler gerçekleştirilebilir.
     */
    private readonly IToDoItemRepository _toDoItemRepository;

    public ToDoItemService(IToDoItemRepository toDoItemRepository)
    {
        _toDoItemRepository = toDoItemRepository;
    }

    /*
     * Belirtilen ID ve kullanıcı ID'sine sahip görevi getirir. Sadece görevin sahibi olan kullanıcı görevi
     * görüntüleyebilir. ToDoItem bulunamazsa null döner.
     */
    public async Task<ToDoItem?> GetTodoItemAsync(int todoItemId, int userId)
    {
        if (todoItemId <= 0)
        {
            throw new ArgumentException("Geçersiz görev kimliği.");
        }
        return await _toDoItemRepository.GetById(todoItemId, userId);
    }

    /*
     * Belirtilen kullanıcıya ait görevleri filtreli şekilde getirir. isCompleted parametresi ile
     * tamamlanmış/tamamlanmamış görevler filtrelenebilir. isCompleted null ise tüm görevleri getirir.
     */
    public async Task<IEnumerable<ToDoItem>?> GetFilteredItemsAsync(int userId, bool? isCompleted = null)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Geçersiz görev kimliği");
        }

        return await _toDoItemRepository.GetFilteredItems(userId, isCompleted);
    }
    
    /*
     * Yeni bir görev ekler. Görev null olamaz ve geçerli bir kullanıcıya ait olmalıdır. İşlem başarılı olursa true döner.
     */
    public async Task<bool> AddAsync(ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            throw new ArgumentNullException(nameof(todoItem), "Görev null olamaz.");
        }

        if (todoItem.UserId <= 0)
        {
            throw new ArgumentException("Görev geçerli bir kullanıcıya ait olmalıdır.");
        }
        try
        {
            await _toDoItemRepository.AddAsync(todoItem);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Görev eklenirken bir hata oluştu.", ex);
        }
    }
    
    /*
     * Var olan bir görevi günceller. Görev null olamaz ve sadece görevin sahibi güncelleyebilir. İşlem başarılı olursa
     true döner.
     */
    public async Task<bool> UpdateAsync(ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            throw new ArgumentNullException(nameof(todoItem), "Görev null olamaz.");
        }

        var existingTodo = await _toDoItemRepository.GetById(todoItem.Id, todoItem.UserId);
        if (existingTodo == null)
        {
            throw new InvalidOperationException(
                "Güncellenecek görev bulunamadı veya bu görevi güncelleme yetkiniz yok.");
        }
        
        try
        {
            _toDoItemRepository.Update(todoItem);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Görev eklenirken bir hata oluştu.", ex);
        }
    }
    
    /*
     * Bir görevi siler (soft delete). Görev null olamaz ve sadece görevin sahibi silebilir. İşlem başarılı olursa true
     * döner.
     */
    public async Task<bool> DeleteAsync(ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            throw new ArgumentNullException(nameof(todoItem), "Görev null olamaz.");
        }

        var existingTodo = await _toDoItemRepository.GetById(todoItem.Id, todoItem.UserId);
        if (existingTodo == null)
        {
            throw new InvalidOperationException("Silinecek görev bulunamadı veya bu görevi silme yetkiniz yok.");
        }
        
        try
        {
            _toDoItemRepository.Delete(todoItem);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Görev eklenirken bir hata oluştu.", ex);
        }
    }
    
    /*
     * SaveChangesAsync metodu, yapılan değişiklikleri (ekleme, güncelleme, silme) veritabanına kaydetmek için
     * kullanılır. Bu metod, UnitOfWork pattern'i kullanarak tüm değişikliklerin tek bir transaction'da kaydedilmesini
     * sağlar. İşlem başarılı ise true, başarısız ise false döner.
     * Asenkron olarak çalışır ve repository katmanındaki SaveChangesAsync metodunu çağırır.
     */
    public async Task<bool> SaveChangesAsync()
    {
        return await _toDoItemRepository.SaveChangesAsync();
    }
}