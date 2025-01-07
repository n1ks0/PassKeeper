using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassKeeper.Core.Entities;

namespace PassKeeper.Data.EntityConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.Name).IsRequired();
        builder.Property(u => u.Password).IsRequired();

        builder.HasMany(u => u.Secrets)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);
    }
}