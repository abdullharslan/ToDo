using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configrution;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable("ToDoItems");
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(t => t.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasOne<User>()
            .WithMany(u => u.ToDoItems) 
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}