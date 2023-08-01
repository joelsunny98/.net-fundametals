namespace RetailStore.Model;

/// <summary>
/// Entity for OrderDetail
/// </summary>
public class OrderDetail: DomainAudit
{

    /// <summary>
    /// Gets and sets Unique Identification number for order entity.
    /// </summary>
    /// <example> 1 </example> 
    public int Id { get; set; }

    /// <summary>
    /// Gets and sets Unique Identification number for order of customer.
    /// </summary>
    /// <example> 1 </example> 
    public int OrderId { get; set; }

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

    /// <summary>
    /// Gets and sets order
    /// </summary>
    public virtual Order Order { get; set; }

    /// <summary>
    /// Gets and sets product
    /// </summary>
    public virtual Product Product { get; set; }
}
