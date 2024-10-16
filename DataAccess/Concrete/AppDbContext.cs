using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete;

/*
 * Uygulamanın veritabanı bağlamı (DbContext) sınıfıdır. Bu sınıf, Entity Framework Core kullanarak veritabanı
 * işlemlerini yönetir. Veritabanındaki Users ve ToDoItems tabloları bu sınıf aracılığıyla temsil edilir. CRUD işlemleri
 * (Create, Read, Update, Delete) için gerekli olan DbSet özellikleri tanımlanmıştır. Ayrıca, veritabanı model
 * yapılandırmaları (tablolar arası ilişkiler, sütun kısıtlamaları) OnModelCreating metodu aracılığıyla yapılır.
 */
public class AppDbContext : DbContext
{
    // Users tablosunu temsil eder
    public DbSet<User> Users { get; set; }
    // ToDoItems tablosunu temsil eder
    public DbSet<ToDoItem> ToDoItems { get; set; }
    
    /*
     * Bu yapılandırıcı, DbContextOptions parametresini alır ve bu parametreyi temel sınıf (base class) olan
     * DbContext'e ileterek veritabanı bağlantısını ve diğer ayarları yapılandırmamızı sağlar. İçerisine bir şey
     * yazılmasına gerek yok. Çünkü DbContextOptions<AppDbContext> parametresi, veritabanı bağlantısı ve diğer ayarların
     * Entity Framework Core tarafından işlenmesi için yeterlidir. Yani, bu yapılandırıcı sadece veritabanı bağlantı
     * bilgilerini temel sınıf olan DbContext'e iletir.
     */
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    /*
     * Veritabanı model yapılandırmalarını özelleştirmek için kullanılır. Bu metot, EF Core'un varsayılan davranışlarını
     * değiştirmemize ve tablolar, ilişkiler ve sütun kısıtlamaları gibi veritabanı modelleme işlemlerini
     * özelleştirmemize olanak tanır. Kullanıcı ve görevler tabloları (User, ToDoItem) için gerekli kısıtlamalar ve
     * ilişkiler burada tanımlanır.
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ToDoItem ve User arasındaki ilişki
        modelBuilder.Entity<ToDoItem>()
            // Her ToDoItem bir User'a ait
            .HasOne<User>()
            // Her User birden fazla ToDoItem'a sahip olabilir
            .WithMany()
            // ToDoItem'daki UserId ile ilişkilendir
            .HasForeignKey(t => t.UserId)
            // User silinirse ona bağlı ToDoItem'lar da silinsin
            .OnDelete(DeleteBehavior.Restrict);
        
        // User için zorunlu alanlar
        modelBuilder.Entity<User>()
            // User sınıfının veritabanındaki karşılık gelen Users tablosu için kurallar ve kısıtlamalar tanımlıyoruz.
            .Property(u => u.Username)
            // Username alanı zorunlu olmalı (null geçerli değil)
            .IsRequired()
            // Username alanı maksimum 255 karakter uzunluğunda olmalı
            .HasMaxLength(255);

        modelBuilder.Entity<User>()
            // User sınıfının veritabanındaki karşılık gelen Users tablosu için kurallar ve kısıtlamalar tanımlıyoruz.
            .Property(u => u.Password)
            // Password alanı zorunlu olmalı (null geçerli değil)
            .IsRequired()
            // Password alanı maksimum 255 karakter uzunluğunda olmalı
            .HasMaxLength(255);

        modelBuilder.Entity<ToDoItem>()
            // User sınıfının veritabanındaki karşılık gelen Users tablosu için kurallar ve kısıtlamalar tanımlıyoruz.
            .Property(t => t.Title)
            // Title (görev başlığı) alanı zorunlu olmalı (null geçerli değil)
            .IsRequired()
            // Title alanı maksimum 255 karakter uzunluğunda olmalı
            .HasMaxLength(255); 
    }
}