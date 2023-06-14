namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for querying order detail table. 
/// </summary>
public class OrderDetailDto
{
    /// <summary>
    /// Gets and sets Unique Identification number for order entity
    /// </summary>
    /// <example> 1 </example> 
    public int Id { get; set; }

    /// <summary>
    /// Gets and sets Unique name of product entity
    /// </summary>
    /// <example> "Soap" </example>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets and sets the product Id for the order.
    /// </summary>
    /// <example> 1 </example> 
    public int ProductId { get; set; }

    /// <summary>
    /// Gets and sets the quantity of product ordered .
    /// </summary>
    /// <example> 5 </example>
    public int Quantity { get; set; }
}
