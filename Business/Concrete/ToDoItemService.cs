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

    // Belirtilen kimliğe sahip görevi getirir.
    public ToDoItem GetTodoItem(int todoItemId)
    {
        // Geçersiz kimlik durumunda bir ArgumentException fırlatılır.
        if (todoItemId <= 0)
        {
            throw new ArgumentException("Geçersiz görev kimliği.");
        }
        return _toDoItemRepository.GetById(todoItemId); 
    }

    // Yeni görev ekler
    public void Add(ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            throw new ArgumentNullException(nameof(todoItem), "Görev null olamaz.");
        }
        _toDoItemRepository.Add(todoItem);
    }
    
    // Görevi günceller
    public void Update(ToDoItem todoItem)
    {
        // Görev nesnesinin null olup olmadığını kontrol eder.
        if (todoItem == null)
        {
            throw new ArgumentNullException(nameof(todoItem), "Görev null olamaz.");
        }
        _toDoItemRepository.Update(todoItem);
    }
    
    // Görevi siler
    public void Delete(ToDoItem todoItem)
    {
        // Görev nesnesinin null olup olmadığını kontrol eder.
        if (todoItem == null)
        {
            throw new ArgumentNullException(nameof(todoItem), "Görev null olamaz.");
        }
        _toDoItemRepository.Delete(todoItem);
    }
    
    // Tüm görevleri getirir
    public IEnumerable<ToDoItem> GetAll()
    {
        return _toDoItemRepository.GetAll();
    }
}