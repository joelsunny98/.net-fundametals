using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetailStore.Model;

namespace RetailStore.Persistence.EntityConfiguration;

/// <summary>
/// Entity Configuration for Order Entity
/// </summary>
public class OrderEntityConfiguration: IEntityTypeConfiguration<Order>
{
    /// <summary>
    /// Method to configure Order Entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CustomerId).IsRequired();
        builder.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId);
    }
}
