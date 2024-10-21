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

    public ToDoItem GetById(int id)
    {
        // Belirtilen ID'ye sahip görevi veritabanından bulur. Bulamazsa, bir InvalidOperationException fırlatır.
        return _appDbContext.ToDoItems.Find(id) ?? throw new InvalidOperationException("Görev bulunamadı.");
    }

    public IEnumerable<ToDoItem> GetAll()
    {
        // Tüm görevleri getir
        return _appDbContext.ToDoItems.ToList();
    }

    public void Add(ToDoItem todoItem)
    {
        // Yeni bir göreve ekler.
        _appDbContext.ToDoItems.Add(todoItem);
        // Değişikliği kaydeder.
        _appDbContext.SaveChanges();
    }

    public void Update(ToDoItem todoItem)
    {
        // Görevi günceller.
        _appDbContext.ToDoItems.Update(todoItem);
        // Değişikliği kaydeder.
        _appDbContext.SaveChanges();
    }

    public void Delete(ToDoItem? todoItem)
    {
        // Kullanıcı nesnesi null ise
        if (todoItem == null)
        {
            // Hiçbir işlem yapma ve çık
            return;
        }
        // Kullanıcıyı sil
        _appDbContext.ToDoItems.Remove(todoItem);
        // Değişikliği kaydet
        _appDbContext.SaveChanges();
    }
}