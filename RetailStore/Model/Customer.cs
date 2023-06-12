namespace RetailStore.Model;

public class Customer: DomainAudit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PhoneNumber { get; set; }
}
