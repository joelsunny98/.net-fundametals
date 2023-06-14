namespace RetailStore.Dtos;

public class BestSellerDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double TotalRevenue { get; set; }
}
