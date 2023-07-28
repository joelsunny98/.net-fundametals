namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object for updating product information.
/// </summary>
public class UpdateProductData
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    /// <example> 10.50 </example>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    /// <example> "Biscut" </example>
    public string? ProductName { get; set; }

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    /// <example> 10.50 </example>
    public decimal ProductPrice { get; set; }
}
