using Entity.Concrete;

namespace DataAccess.Abstract;

/*
 * ITodoItemRepository arayüzü, görev işlemleri için gerekli CRUD (Create, Read, Update, Delete) metotlarını tanımlar.
 * Bu arayüz, görevlerin veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 */
public interface IToDoItemRepository
{
    ToDoItem? GetById(int id);
    IEnumerable<ToDoItem> GetFilteredItems(bool? isCompleted = null); 
    void Add(ToDoItem todoItem);
    void Update(ToDoItem todoItem);
    void Delete(ToDoItem todoItem);
}