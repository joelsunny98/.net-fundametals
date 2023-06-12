namespace RetailStore.Model;

public class Product: DomainAudit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}
