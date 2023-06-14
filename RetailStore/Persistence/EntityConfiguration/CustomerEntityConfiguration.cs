using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetailStore.Model;

namespace RetailStore.Persistence.EntityConfiguration
{
    public class CustomerEntityConfiguration: IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        }
    }
}
