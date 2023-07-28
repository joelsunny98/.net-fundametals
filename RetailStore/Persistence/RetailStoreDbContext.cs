using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Model;

namespace RetailStore.Persistence;

/// <summary>
/// Context of Retail Store Database
/// </summary>
public class RetailStoreDbContext: DbContext, IRetailStoreDbContext
{
    /// <summary>
    /// Builder for RetailStoreDbContext
    /// </summary>
    /// <param name="options"></param>
    public RetailStoreDbContext(DbContextOptions<RetailStoreDbContext> options): base(options) { }

    /// <summary>
    /// Gets and sets Products
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets and sets Orders
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets and sets OrderDetails
    /// </summary>
    public DbSet<OrderDetail> OrderDetails { get; set; }

    /// <summary>
    /// Gets and sets Customers
    /// </summary>
    public DbSet<Customer> Customers { get; set; }

    /// <summary>
    /// Method to build Model
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RetailStoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Runs pending database migrations asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RunMigrations()
    {
        await Database.MigrateAsync();
    }
}
