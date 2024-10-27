using Entity.Concrete;

namespace Business.Abstract;

// Kullanıcılar ile ilgili iş kurallarını tanımlayacak arayüz
public interface IUserService
{
    Task<User?> GetUserAsync(int userId);
    Task<bool> AddAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> SaveChangesAsync();
}