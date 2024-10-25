using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete;

/*
   * ToDoItemRepository sınıfı, görev nesneleri üzerinde CRUD (Create, Read, Update, Delete) işlemlerini 
   * gerçekleştiren bir veri erişim katmanıdır. Bu sınıf, AppDbContext nesnesini kullanarak veritabanındaki 
   * ToDoItems tablosu ile etkileşimde bulunur. Her bir metod, belirli görevleri bulmak, eklemek, güncellemek 
   * ve silmek için gerekli işlevselliği sağlar.
   */
public class ToDoItemRepository : IToDoItemRepository
{
    /*
     * ToDoItemRepository sınıfının yapıcı metodu, dışarıdan bir AppDbContext nesnesi alır ve bu nesneyi
     * _appDbContext değişkenine atar. Bu sayede sınıf içinde görevler ile ilgili veritabanı işlemleri için
     * kullanılacak DbContext nesnesine erişim sağlanır.
     */
    private readonly AppDbContext _appDbContext;

    public ToDoItemRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // Belirtilen ID'ye sahip görevi veritabanından bulur ve döndürür.
    public ToDoItem? GetById(int id)
    {
        return _appDbContext.ToDoItems.Find(id);
    }
    
    // Belirtilen filtreye göre görevleri döndürür. Eğer filtre belirtilmemişse (null) tüm görevleri listeler.
    public IEnumerable<ToDoItem> GetFilteredItems(bool? isCompleted = null)
    {
        return isCompleted == null
            ? _appDbContext.ToDoItems.ToList()
            : _appDbContext.ToDoItems
                .Where(item => item.IsCompleted == isCompleted)
                .ToList();
    }
    
    // Yeni bir görevi veritabanına ekler.
    public void Add(ToDoItem todoItem)
    {
        _appDbContext.ToDoItems.Add(todoItem);
        _appDbContext.SaveChanges();
    }

    // Var olan bir görevi günceller.
    public void Update(ToDoItem todoItem)
    {
        _appDbContext.ToDoItems.Update(todoItem);
        _appDbContext.SaveChanges();
    }

    // Verilen görevi siler.
    public void Delete(ToDoItem? todoItem)
    {
        _appDbContext.ToDoItems.Remove(todoItem);
        _appDbContext.SaveChanges();
    }
}