namespace RetailStore.Model;

/// <summary>
/// Entity for Order
/// </summary>
public class Order: DomainAudit
{
    /// <summary>
    /// Gets and sets Unique Identification number for order entity
    /// </summary>
    /// <example> 1 </example> 
    public int  Id { get; set; }

    /// <summary>
    /// Gets and sets Unique Identification number of the customer entity
    /// </summary>
    /// <example> 1 </example> 

    public int CustomerId { get; set; }

    /// <summary>
    /// Gets and sets total amount of the order
    /// </summary>
    /// <example> 110.5 </example> 
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets and sets discount amount of the order
    /// </summary>
    /// <example> 110.5 </example> 
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets and sets Customer
    /// </summary>
    public virtual Customer Customer { get; set; }

    /// <summary>
    /// Gets and sets the details of order.
    /// </summary>
    public List<OrderDetail> Details { get; set;}

}
