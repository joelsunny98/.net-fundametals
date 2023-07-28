namespace RetailStore.Dtos;

public class ProductDto
{
    /// <summary>
    /// Gets and sets Product Name
    /// </summary>
    /// <example> "Soap" </example>
    public string? ProductName { get; set; }

    /// <summary>
    /// Gets and sets Product Price
    /// </summary>
    /// <example> 10.50 </example>
    public decimal ProductPrice { get; set; }
}
