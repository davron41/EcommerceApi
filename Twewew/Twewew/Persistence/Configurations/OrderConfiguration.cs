using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twewew.Entities;
using Twewew.Enums;

namespace Twewew.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));
        builder.HasKey(o => o.Id);


        builder
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);


        builder
            .Property(o => o.OrderDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(o => o.TotalQuantity)
            .HasColumnType("decimal")
            .IsRequired();

        builder
            .Property(o => o.OrderStatus)
            .HasDefaultValue(OrderStatus.Pending)
            .IsRequired();

    }
}
