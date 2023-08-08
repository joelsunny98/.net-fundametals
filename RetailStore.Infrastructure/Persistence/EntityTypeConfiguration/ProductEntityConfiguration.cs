using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetailStore.Model;

namespace RetailStore.Persistence.EntityConfiguration;

/// <summary>
/// Entity configuration for Product Entity
/// </summary>
public class ProductEntityConfiguration: IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Method to configure Product Entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Product> builder) 
    {
        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd();
    }
}
