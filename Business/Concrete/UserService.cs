using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete;

/*
 * UserService sınıfı, IUserService arayüzünü implement ederek kullanıcılarla ilgili iş mantığını yönetir. Bu sınıf,
 * kullanıcıların eklenmesi, güncellenmesi, silinmesi ve kullanıcı bilgilerini alma işlemlerini gerçekleştirir. Ayrıca,
 * iş kurallarını uygulayarak veritabanı ile etkileşimde bulunur.
 */
public class UserService : IUserService
{
    /*
     * UserService sınıfının yapıcı metodu, dışarıdan bir IUserRepository nesnesi alır ve bu nesneyi _userRepository
     * değişkenine atar. Bu sayede sınıf içinde kullanıcı işlemleri için kullanılacak repository nesnesine erişim
     * sağlanır ve kullanıcıların eklenmesi, güncellenmesi, silinmesi gibi işlemler gerçekleştirilebilir.
     */
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Kullanıcı kimliğini getirir
    public User? GetUser(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Geçersiz kullanıcı kimliği.");
        }
        return _userRepository.GetById(userId);
    }

    // Kullanıcı ekler
    public void Add(User user)
    {
        // Kullanıcı nesnesinin null olup olmadığını kontrol eder.
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        if (_userRepository.GetByUsername(user.Username) != null)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten mevcut.");
        }

        try
        {
            _userRepository.Add(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Kullanıcı '{user.Username}' eklenirken bir hata oluştu. Lütfen tekrar deneyin.", ex);
        }
        
    }
    
    // Kullanıcıyı günceller
    public void Update(User user)
    {
        // Kullanıcı nesnesinin null olup olmadığını kontrol eder.
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        var existingUser = _userRepository.GetByUsername(user.Username);
        if (existingUser != null && existingUser.Id != user.Id)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten mevcut.");
        }
        
        try
        {
            _userRepository.Update(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Kullanıcı '{user.Username}' eklenirken bir hata oluştu. Lütfen tekrar deneyin.", ex);
        }
    }

    // Kullanıcıyı siler
    public void Delete(User user)
    {
        // Kullanıcı nesnesinin null olup olmadığını kontrol eder.
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        
        try
        {
            _userRepository.Delete(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Kullanıcı '{user.Username}' eklenirken bir hata oluştu. Lütfen tekrar deneyin.", ex);
        }
    }
}