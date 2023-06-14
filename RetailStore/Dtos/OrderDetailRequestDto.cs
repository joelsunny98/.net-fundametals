namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for  order details table. 
/// </summary>
public class OrderDetailRequestDto
{
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
