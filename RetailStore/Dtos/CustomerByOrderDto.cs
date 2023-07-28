namespace RetailStore.Dtos;

public class CustomerByOrderDto
{
    /// <summary>
    /// Gets and sets CustomerName
    /// </summary>
    /// <example> "Ravi" </example>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets and sets Total Orders
    /// </summary> 
    /// <example> 10 </example>
    public int TotalOrders { get; set; }
}
