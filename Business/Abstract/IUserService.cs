using Entity.Concrete;

namespace Business.Abstract;

// Kullanıcılar ile ilgili iş kurallarını tanımlayacak arayüz
public interface IUserService
{
    User? GetUser(int userId);
    void Add(User user);
    void Update(User user);
    void Delete(User user);
}