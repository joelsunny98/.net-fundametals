using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetailStore.Model;

namespace RetailStore.Persistence.EntityConfiguration;

/// <summary>
/// Entity Configuration for Customer
/// </summary>
public class CustomerEntityConfiguration: IEntityTypeConfiguration<Customer>
{
    /// <summary>
    /// Method to configure Customer Entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd();
    }
}
