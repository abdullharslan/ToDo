using Entity.Concrete;

namespace Business.Abstract;

// Görevler ile ilgili iş kurallarını tanımlayacak arayüz
public interface IToDoItemService
{
    Task <ToDoItem?> GetTodoItemAsync(int todoItemId, int userId);
    Task <IEnumerable<ToDoItem>?> GetFilteredItemsAsync(int userId, bool? isCompleted = null);
    Task<bool> AddAsync(ToDoItem todoItem);
    Task<bool> UpdateAsync(ToDoItem todoItem);
    Task<bool> DeleteAsync(ToDoItem todoItem);
    Task<bool> SaveChangesAsync();
}