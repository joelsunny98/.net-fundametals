using System.ComponentModel.DataAnnotations;

namespace RetailStore.Model;

public class Customer: DomainAudit
{
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    public long? PhoneNumber { get; set; }
}
