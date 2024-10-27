using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ToDoItem?> GetById(int id, int userId)
    {
        return await _appDbContext.ToDoItems
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }
    
    // Belirtilen filtreye göre görevleri döndürür. Eğer filtre belirtilmemişse (null) tüm görevleri listeler.
    public async Task<IEnumerable<ToDoItem>> GetFilteredItems(int userId, bool? isCompleted = null)
    {
        var query = _appDbContext.ToDoItems
            .Include(t => t.User)
            .Where(t => t.UserId == userId);

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }
        
        return await query.ToListAsync();
    }
    
    // Yeni bir görevi veritabanına ekler.
    public async Task AddAsync(ToDoItem todoItem)
    {
        await _appDbContext.ToDoItems.AddAsync(todoItem);
    }

    // Var olan bir görevi günceller.
    public void Update(ToDoItem todoItem)
    {
        _appDbContext.ToDoItems.Update(todoItem);
    }

    // Verilen görevi siler.
    public void Delete(ToDoItem todoItem)
    {
        // Fiziksel silme yerine soft delete
        todoItem.IsDeleted = true;
        _appDbContext.ToDoItems.Remove(todoItem);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync() > 0;
    }
}