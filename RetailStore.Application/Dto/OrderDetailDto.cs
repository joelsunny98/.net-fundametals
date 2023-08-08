namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for querying order detail table. 
/// </summary>
public class OrderDetailDto
{

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
}
