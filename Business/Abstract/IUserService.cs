using Entity.Concrete;

namespace Business.Abstract;

// Kullanıcılar ile ilgili iş kurallarını tanımlayacak arayüz
public interface IUserService
{
    // Kullanıcıyı kimliği ile getir
    User GetUser(int userId);
    // Yeni kullanıcı ekle
    void Add(User user);
    // Kullanıcıyı güncelle
    void Update(User user);
    // Kullanıcıyı sil
    void Delete(User user);
}