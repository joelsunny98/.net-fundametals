using Microsoft.EntityFrameworkCore;
using RetailStore.Model;

namespace RetailStore.Contracts;

/// <summary>
/// Interface for access to the data store for managing products and related entities in the retail store.
/// </summary>
public interface IRetailStoreDbContext
{
    /// <summary>
    /// Gets and sets Products
    /// </summary>
    DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets and sets Orders
    /// </summary>
    DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets and sets OrderDetails
    /// </summary>
    DbSet<OrderDetail> OrderDetails { get; set; }

    /// <summary>
    /// Gets and sets Customers
    /// </summary>
    DbSet<Customer> Customers { get; set; }

    Task RunMigrations();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
