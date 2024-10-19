using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete;

/*
 * UserService sınıfı, IUserService arayüzünü implement ederek kullanıcılarla ilgili iş mantığını yönetir. Bu sınıf,
 * kullanıcıların eklenmesi, güncellenmesi, silinmesi ve kullanıcı bilgilerini alma işlemlerini gerçekleştirir. Ayrıca,
 * iş kurallarını uygulayarak veritabanı ile etkileşimde bulunur.
 */
public class UserService(IUserRepository userRepository) : IUserService
{
    /*
     * UserService sınıfının yapıcı metodu, dışarıdan bir IUserRepository nesnesi alır ve bu nesneyi _userRepository
     * değişkenine atar. Bu sayede sınıf içinde kullanıcı işlemleri için kullanılacak repository nesnesine erişim
     * sağlanır ve kullanıcıların eklenmesi, güncellenmesi, silinmesi gibi işlemler gerçekleştirilebilir.
     */
    private readonly IUserRepository _userRepository = userRepository;
    
    // Kullanıcı kimliğini getirir
    public User GetUser(int userId)
    {
        // Geçersiz kullanıcı kimliği durumunda bir istisna fırlatılır.
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
        /*
         * Kullanıcıyı veritabanına ekler. Bu işlem yan etki yaratır ve ekleme sonucunu döndürmez; işlem tamamlandığında
         * kullanıcı kaydı veritabanında güncellenir.
         * İş kurallarını burada uygula (örneğin, kullanıcı adı benzersiz olmalı)
         */

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_userRepository.GetByUsername(user.Username) != null)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten mevcut.");
        }
        _userRepository.Add(user);
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
        _userRepository.Update(user);
    }

    // Kullanıcıyı siler
    public void Delete(User user)
    {
        // Kullanıcı nesnesinin null olup olmadığını kontrol eder.
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        _userRepository.Delete(user);
    }
}