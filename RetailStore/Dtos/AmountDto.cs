﻿namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object for storing discounted amount and discount value.
/// </summary>
public class AmountDto
{
    /// <summary>
    /// Gets or sets the discounted amount.
    /// </summary>
    /// <example> 100.50 </example>
    public decimal DiscountedAmount { get; set; }

    /// <summary>
    /// Gets or sets the discount value.
    /// </summary>
    /// <example> 10.50 </example>
    public decimal DiscountValue { get; set; }
}
