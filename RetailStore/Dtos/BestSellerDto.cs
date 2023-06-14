namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for querying Best seller product from order table. 
/// </summary>
public class BestSellerDto
{

    /// <summary>
    /// Gets and sets the product Id for the order.
    /// </summary>
    /// <example> 1 </example> 
    public int ProductId { get; set; }

    /// <summary>
    /// Gets and sets Unique name of product entity
    /// </summary>
    /// <example> "Soap" </example>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets and sets the quantity of product ordered .
    /// </summary>
    /// <example> 10 </example>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets and sets the price of product ordered .
    /// </summary>
    /// <example> 10.50 </example>
    public double Price { get; set; }

    /// <summary>
    /// Gets and sets the total price of products that ordered .
    /// </summary>
    /// <example> 100.50 </example>
    public double TotalRevenue { get; set; }
}
