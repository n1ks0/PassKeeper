using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassKeeper.Core.Entities;

namespace PassKeeper.Data.EntityConfigurations;

public class SecretEntityTypeConfiguration : IEntityTypeConfiguration<Secret>
{
    public void Configure(EntityTypeBuilder<Secret> builder)
    {
        builder.ToTable("Secrets");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Key).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(u => u.Secrets)
            .HasForeignKey(x => x.UserId);
    }
}