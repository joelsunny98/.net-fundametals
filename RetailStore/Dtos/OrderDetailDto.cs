namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for querying order detail table. 
/// </summary>
public class OrderDetailDto
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
