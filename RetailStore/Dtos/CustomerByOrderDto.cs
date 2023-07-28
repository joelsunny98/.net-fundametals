namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object for storing customer name and total orders.
/// </summary>
public class CustomerByOrderDto
{
    /// <summary>
    /// Gets or sets the customer's name.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the total number of orders.
    /// </summary>
    public int TotalOrders { get; set; }
}
