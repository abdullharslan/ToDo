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

    /*
     * GetUserAsync metodu, belirtilen kimliğe sahip kullanıcıyı veritabanından asenkron olarak getirir. UserId
     * parametresinin 0'dan büyük olması gerekir, aksi halde ArgumentException fırlatır. Kullanıcı bulunamazsa null
     * döner.
     */
    public async Task<User?> GetUserAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Geçersiz kullanıcı kimliği.");
        }
        return await _userRepository.GetById(userId);
    }

    /*
     * GetUserByUsernameAsync metodu, belirtilen kullanıcı adına sahip kullanıcıyı veritabanından asenkron olarak
     * getirir. Username parametresi boş olamaz, aksi halde ArgumentException fırlatır. Kullanıcı bulunamazsa null döner.
     */
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Kullanıcı adı boş bırakılamaz.");
        }
        return await _userRepository.GetByUsername(username);
    }
    
    /*
     * AddAsync metodu, yeni bir kullanıcıyı veritabanına asenkron olarak ekler. User parametresi null olamaz, aksi
     * halde ArgumentNullException fırlatır. Aynı kullanıcı adına sahip başka bir kullanıcı varsa
     * InvalidOperationException fırlatır. İşlem başarılı ise true döner, hata durumunda InvalidOperationException
     * fırlatır.
     */
    public async Task<bool> AddAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        
        var existingUser = await _userRepository.GetByUsername(user.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten mevcut.");
        }

        try
        {
            await _userRepository.AddAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Kullanıcı '{user.Username}' eklenirken bir hata oluştu. Lütfen tekrar deneyin.", ex);
        }
    }
    
    /*
     * UpdateAsync metodu, var olan bir kullanıcının bilgilerini veritabanında asenkron olarak günceller.
     * User parametresi null olamaz, aksi halde ArgumentNullException fırlatır. Güncellenecek kullanıcı adı başka bir
     * kullanıcı tarafından kullanılıyorsa InvalidOperationException fırlatır. İşlem başarılı ise true döner, hata
     * durumunda InvalidOperationException fırlatır.
     */
    public async Task<bool> UpdateAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        
        var existingUser = await _userRepository.GetByUsername(user.Username);
        if (existingUser != null && existingUser.Id != user.Id)
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten mevcut.");
        }
        
        try
        {
            _userRepository.Update(user);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Kullanıcı '{user.Username}' eklenirken bir hata oluştu. Lütfen tekrar deneyin.", ex);
        }
    }

    /*
     * DeleteAsync metodu, belirtilen kullanıcıyı veritabanından asenkron olarak siler (soft delete). User parametresi
     * null olamaz, aksi halde ArgumentNullException fırlatır. İşlem başarılı ise true döner, hata durumunda
     * InvalidOperationException fırlatır.
     */
    public async Task<bool> DeleteAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Kullanıcı null olamaz.");
        }
        
        try
        {
            _userRepository.Delete(user);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Kullanıcı '{user.Username}' eklenirken bir hata oluştu. Lütfen tekrar deneyin.", ex);
        }
    }

    /*
     * SaveChangesAsync metodu, yapılan değişiklikleri (ekleme, güncelleme, silme) veritabanına kaydetmek için
     * kullanılır. Bu metod, UnitOfWork pattern'i kullanarak tüm değişikliklerin tek bir transaction'da kaydedilmesini
     * sağlar. İşlem başarılı ise true, başarısız ise false döner.
     * Asenkron olarak çalışır ve repository katmanındaki SaveChangesAsync metodunu çağırır.
     */
    public async Task<bool> SaveChangesAsync()
    {
        return await _userRepository.SaveChangesAsync();
    }
}