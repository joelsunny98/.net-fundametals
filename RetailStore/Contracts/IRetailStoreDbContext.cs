using Microsoft.EntityFrameworkCore;
using RetailStore.Model;

namespace RetailStore.Contracts;

/// <summary>
/// Interface for RetailStoreDbContext
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

    /// <summary>
    /// Method to run Migration
    /// </summary>
    Task RunMigrations();

    /// <summary>
    /// Method to Save db changes
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
