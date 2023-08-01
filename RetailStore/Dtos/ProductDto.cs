namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object (DTO) class for representing a product.
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    /// <example> "Soap" </example>
    public string? ProductName { get; set; }

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    /// <example> 10.50 </example>
    public decimal ProductPrice { get; set; }
}
