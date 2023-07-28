namespace RetailStore.Dtos;

public class UpdateProductData
{
    public int Id { get; set; }
    /// <summary>
    /// Gets and sets Product Name
    /// </summary>
    /// <example> 6 </example>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets and sets Product Price
    /// </summary>
    /// <example> 10.50 </example>
    public decimal ProductPrice { get; set; }
}
