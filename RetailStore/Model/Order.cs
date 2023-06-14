namespace RetailStore.Model;

public class Order: DomainAudit
{
    public int  Id { get; set; }
    public int CustomerId { get; set; }
    public double TotalAmount { get; set; }
    public virtual Customer Customer { get; set; }
    public List<OrderDetail> Details { get; set;}

}
