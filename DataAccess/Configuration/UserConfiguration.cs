using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configrution;

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
        
        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(10);;

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);
    }
}