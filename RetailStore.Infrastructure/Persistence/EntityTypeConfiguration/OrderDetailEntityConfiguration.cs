using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetailStore.Model;

namespace RetailStore.Persistence.EntityConfiguration;

/// <summary>
/// Entity Configuration for Order Detail entity
/// </summary>
public class OrderDetailEntityConfiguration: IEntityTypeConfiguration<OrderDetail>
{
    /// <summary>
    /// Method to configure OrderDetail Entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<OrderDetail> builder) 
    {
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.OrderId).IsRequired();
        builder.HasOne(e => e.Order)
            .WithMany()
            .HasForeignKey(e => e.OrderId);

        builder.Property(e => e.ProductId).IsRequired();
        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId);

        builder.HasOne(e => e.Order)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.OrderId)
            .HasPrincipalKey(e => e.Id);
    }
}
