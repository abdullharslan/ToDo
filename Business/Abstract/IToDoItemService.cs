using Entity.Concrete;

namespace Business.Abstract;

// Görevler ile ilgili iş kurallarını tanımlayacak arayüz
public interface IToDoItemService
{
    ToDoItem? GetTodoItem(int todoItemId);
    void Add(ToDoItem todoItem);
    void Update(ToDoItem todoItem);
    void Delete(ToDoItem todoItem);
    IEnumerable<ToDoItem>? GetFilteredItems(bool? isCompleted = null);
}