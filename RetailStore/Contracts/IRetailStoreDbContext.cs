﻿using Microsoft.EntityFrameworkCore;
using RetailStore.Model;

namespace RetailStore.Contracts;

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

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
