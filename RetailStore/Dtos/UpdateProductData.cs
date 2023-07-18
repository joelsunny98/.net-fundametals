namespace RetailStore.Dtos;

public class UpdateProductData
{
    public int Id { get; set; }
    /// <summary>
    /// Gets and sets Product Name
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets and sets Product Price
    /// </summary>
    public decimal ProductPrice { get; set; }
}
