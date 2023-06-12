using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetailStore.Model;

namespace RetailStore.Persistence.EntityConfiguration;

public class OrderDetailEntityConfiguration: IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder) 
    {
        builder.Property(e => e.OrderId).IsRequired();
        builder.HasOne(e => e.Order)
            .WithMany()
            .HasForeignKey(e => e.OrderId);

        builder.Property(e => e.ProductId).IsRequired();
        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId);
    }
}
