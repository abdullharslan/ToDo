using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete;

public class UserRepository : IUserRepository
{
    /*
     * UserRepository sınıfının yapıcı metodu, dışarıdan bir AppDbContext nesnesi alır ve bu nesneyi _appDbContext
     * değişkenine atar. Bu sayede sınıf içinde veritabanı işlemleri için kullanılacak DbContext nesnesine erişim
     * sağlanır.
     */
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // Belirtilen ID'ye sahip kullanıcıyı getirir.
    public async Task<User?> GetById(int id)
    {
        return await _appDbContext.Users
            .Include(u => u.ToDoItems)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    // Kullanıcı adına göre kullanıcıyı getirir
    public async Task<User?> GetByUsername(string username)
    {
        return await _appDbContext.Users
            .Include(u => u.ToDoItems)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    // Yeni kullanıcı ekler
    public async Task AddAsync(User user)
    {
        await _appDbContext.Users.AddAsync(user);
    }

    // Belirtilen ID'ye sahip kullanıcıyı getirir.
    public async Task<User?> GetByUsername(int id)
    {
        return await _appDbContext.Users
            .Include(u => u.ToDoItems)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    // Kullanıcıyı günceller
    public void Update(User user)
    {
        _appDbContext.Users.Update(user);
    }
    
    // Kullanıcıyı siler
    public void Delete(User user)
    {
        user.IsDeleted = true;
        _appDbContext.Users.Remove(user);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync() > 0;
    }
}