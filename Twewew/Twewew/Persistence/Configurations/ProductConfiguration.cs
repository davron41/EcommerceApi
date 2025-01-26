using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twewew.Entities;

namespace Twewew.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));
        builder.HasKey(p => p.Id);

        builder
            .HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(p => p.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(p => p.Description)
            .HasMaxLength(255);

        builder
            .Property(p => p.Price)
            .HasColumnType("decimal")
            .IsRequired();

        builder
            .Property(p => p.AddedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(p => p.ImageUrl)
            .HasColumnType("bytea");

        builder
            .Property(p => p.IsFrozen)
            .HasDefaultValue(false)
            .IsRequired();


    }
}
