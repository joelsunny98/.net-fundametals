using System.ComponentModel.DataAnnotations;

namespace RetailStore.Model;

public class Product: DomainAudit
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public double Price { get; set; }
}
