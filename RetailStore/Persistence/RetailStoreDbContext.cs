using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Model;

namespace RetailStore.Persistence;

public class RetailStoreDbContext: DbContext, IRetailStoreDbContext
{
    public RetailStoreDbContext(DbContextOptions<RetailStoreDbContext> options): base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RetailStoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
