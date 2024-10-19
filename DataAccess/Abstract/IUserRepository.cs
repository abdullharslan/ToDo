using Entity.Concrete;

namespace DataAccess.Abstract;

/*
 * IUserRepository arayüzü, kullanıcı işlemleri için gerekli CRUD (Create, Read, Update, Delete) metotlarını tanımlar.
 * Bu arayüz, kullanıcıların veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 */
public interface IUserRepository
{
    // Belirli bir kullanıcıyı getir
    User GetById(int id);
    // Yeni bir kullanıcı ekle
    User GetByUsername(string userName);
    void Add(User user);
    // Kullanıcıyı güncelle
    void Update(User user);
    // Kullanıcıyı sil
    void Delete(User user);
}