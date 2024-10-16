using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    /*
     * UserRepository sınıfının yapıcı metodu, dışarıdan bir AppDbContext nesnesi alır ve bu nesneyi _appDbContext
     * değişkenine atar. Bu sayede sınıf içinde veritabanı işlemleri için kullanılacak DbContext nesnesine erişim
     * sağlanır.
     */
    private readonly AppDbContext _appDbContext = appDbContext;

    // Verilen ID ile kullanıcıyı veritabanından bulur. Kullanıcı bulunamazsa, bir InvalidOperationException fırlatır.
    public User GetById(int id)
    {
        return _appDbContext.Users.Find(id) ?? throw new InvalidOperationException();
    }
    
    public void Add(User user)
    {
        // Veritabanına yeni kullanıcı ekler
        _appDbContext.Users.Add(user);
        // Değişikliği kaydeder
        _appDbContext.SaveChanges();
    }
    
    public void Update(User user)
    {
        // Kullanıcıyı günceller
        _appDbContext.Users.Update(user);
        // Değişikliği kaydeder
        _appDbContext.SaveChanges();
    }
    
    public void Delete(User? user)
    {
        // Kullanıcı nesnesi null ise
        if (user == null)
        {
            // Hiçbir şey yapma, çık
            return;
        }
        // Kullanıcıyı veritabanından sil
        _appDbContext.Users.Remove(user);
        // Değişikliği kaydeder
        _appDbContext.SaveChanges();
    }
}