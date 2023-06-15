namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for creating order. 
/// </summary>
public class OrderRequestDto
{
    /// <summary>
    /// Gets and sets Unique Identification number of the customer entity
    /// </summary>
    /// <example> 1 </example> 
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets and sets total amount of the order
    /// </summary>
    /// <example> 110.5 </example> 
    public int TotalAmount { get; set; }

    /// <summary>
    /// Gets and sets the details of order.
    /// </summary>
    public List<OrderDetailRequestDto> Details { get; set; }
}
