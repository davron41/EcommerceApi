using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twewew.Entities;

namespace Twewew.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken));

        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Token)
            .IsUnique();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder
            .Property(x => x.Token)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Navigation(x => x.User)
            .AutoInclude();
    }
}
