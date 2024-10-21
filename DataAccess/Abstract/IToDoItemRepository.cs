using Entity.Concrete;

namespace DataAccess.Abstract;

/*
 * ITodoItemRepository arayüzü, görev işlemleri için gerekli CRUD (Create, Read, Update, Delete) metotlarını tanımlar.
 * Bu arayüz, görevlerin veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 */
public interface IToDoItemRepository
{
    // Belirli bir görevi getir
    ToDoItem GetById(int id);
    // Tüm görevleri getir
    IEnumerable<ToDoItem> GetAll(); 
    // Yeni bir görev ekle
    void Add(ToDoItem todoItem);
    // Görevi güncelle
    void Update(ToDoItem todoItem);
    // Görevi sil
    void Delete(ToDoItem todoItem);
}