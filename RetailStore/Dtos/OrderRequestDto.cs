namespace RetailStore.Dtos;

public class OrderRequestDto
{
    public int CustomerId { get; set; }
    public int TotalAmount { get; set; }
    public List<OrderDetailRequestDto> Details { get; set; }
}
