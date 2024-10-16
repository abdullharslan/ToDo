/*
 * Entity katmanı, veritabanındaki tabloları ve uygulamanın veri modellerini temsil eder.
 * Kullanıcı ve yapılacaklar gibi varlıkların tanımlandığı yerdir.
 */
namespace Entity.Abstract;

// IUser arayüzü, kullanıcı nesneleri için temel özellikleri tanımlar.
public interface IUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}