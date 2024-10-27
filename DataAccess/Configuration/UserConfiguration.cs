using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);;
       
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(70);;
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);
        
        builder.Property(u => u.UpdatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);
        
        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasQueryFilter(u => !u.IsDeleted);
        
        // ToDoItems ilişkisi entity'de tanımlandığı için burada tekrar tanımlamaya gerek yok
    }
}