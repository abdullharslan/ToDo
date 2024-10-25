using System.Reflection;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete;

/*
 * Uygulamanın veritabanı bağlamı (DbContext) sınıfıdır.Entity Framework Core kullanarak veritabanı işlemlerini yönetir.
 * Bu sınıf Users ve ToDoItems tablolarını DbSet özellikleri ile temsil eder. CRUD işlemleri (Create, Read, Update,
 * Delete) için gerekli olan yapılandırmaları içerir.
 */
public class AppDbContext : DbContext
{
 public DbSet<User> Users { get; set; }
 public DbSet<ToDoItem> ToDoItems { get; set; }

 /*
  * Bu yapılandırıcı, DbContextOptions parametresini alır ve bu parametreyi temel sınıf (base class) olan
  * DbContext'e ileterek veritabanı bağlantısını ve diğer ayarları yapılandırmamızı sağlar. Veritabanı bağlantı
  * bilgileri, EF Core tarafından işlenir.
  */
 public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
 {
 }
 
 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
  // Veritabanı modeli için yapılandırmaları uygulamak üzere temel sınıfın metodunu çağır.
  base.OnModelCreating(modelBuilder);
  /*
   * Bu satır, mevcut derlemedeki tüm IEntityTypeConfiguration uygulamalarını otomatik olarak uygulayarak kullanıcı ve
   * görev tablolarının yapılandırmalarını yükler.
   */
  modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
 }
}