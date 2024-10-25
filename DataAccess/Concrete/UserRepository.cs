using DataAccess.Abstract;
using Entity.Concrete;

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

    // Verilen ID ile kullanıcıyı veritabanından bulur. Kullanıcı bulunamazsa, bir InvalidOperationException fırlatır.
    public User? GetById(int id)
    {
        return _appDbContext.Users.Find(id);
    }

    // Kullanıcıyı arar ve getirir, eğer kullanıcıyı bulamazsa hata mesajı yollar.
    public User? GetByUsername(string userName)
    {
        return _appDbContext.Users.SingleOrDefault(u => userName == u.Username);
    }

    // Kullanıcı ekler
    public void Add(User user)
    {
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
    }
    
    // Kullanıcıyı günceller
    public void Update(User user)
    {
        _appDbContext.Users.Update(user);
        _appDbContext.SaveChanges();
    }
    
    // Kullanıcıyı siler
    public void Delete(User? user)
    {
        _appDbContext.Users.Remove(user);
        _appDbContext.SaveChanges();
    }
}