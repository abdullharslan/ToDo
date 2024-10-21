using Entity.Concrete;

namespace Business.Abstract;

// Görevler ile ilgili iş kurallarını tanımlayacak arayüz
public interface IToDoItemService
{
    // Belirli bir görevi kimliği ile getir
    ToDoItem GetTodoItem(int todoItemId);
    // Yeni bir görev ekle
    void Add(ToDoItem todoItem);
    // Mevcut bir görevi güncelle
    void Update(ToDoItem todoItem);
    // Görevi sil
    void Delete(ToDoItem todoItem);
    // Tüm görevleri getir
    IEnumerable<ToDoItem> GetAll();
}