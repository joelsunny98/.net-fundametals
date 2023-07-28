namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object for updating product information.
/// </summary>
public class UpdateProductData
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal ProductPrice { get; set; }
}
