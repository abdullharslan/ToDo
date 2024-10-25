using Entity.Concrete;

namespace DataAccess.Abstract;

/*
 * IUserRepository arayüzü, kullanıcı işlemleri için gerekli CRUD (Create, Read, Update, Delete) metotlarını tanımlar.
 * Bu arayüz, kullanıcıların veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 */
public interface IUserRepository
{
    User? GetById(int id);
    User? GetByUsername(string userName);
    void Add(User user);
    void Update(User user);
    void Delete(User user);
}