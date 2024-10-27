using Entity.Concrete;

namespace DataAccess.Abstract;

/*
 * ITodoItemRepository arayüzü, görev işlemleri için gerekli CRUD (Create, Read, Update, Delete) metotlarını tanımlar.
 * Bu arayüz, görevlerin veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 */
public interface IToDoItemRepository
{
    Task<ToDoItem?> GetById(int id, int userId);
    Task<IEnumerable<ToDoItem>> GetFilteredItems(int userId, bool? isCompleted = null); 
    Task AddAsync(ToDoItem todoItem);
    void Update(ToDoItem todoItem);
    void Delete(ToDoItem todoItem);
    Task<bool> SaveChangesAsync();
}