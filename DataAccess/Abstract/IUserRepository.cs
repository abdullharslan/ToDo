using Entity.Concrete;

namespace DataAccess.Abstract;

/*
 * IUserRepository arayüzü, kullanıcı işlemleri için gerekli CRUD (Create, Read, Update, Delete) metotlarını tanımlar.
 * Bu arayüz, kullanıcıların veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 */
public interface IUserRepository
{
    Task<User?> GetById(int id);
    Task<User?> GetByUsername(string userName);
    Task AddAsync(User user);
    void Update(User user);
    void Delete(User user);
    Task<bool> SaveChangesAsync();
}