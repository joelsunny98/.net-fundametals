using System.ComponentModel.DataAnnotations;

namespace RetailStore.Model;

public class Product: DomainAudit
{

    /// <summary>
    /// Gets and sets Unique Identification number of the product entity
    /// </summary>
    /// <example> 1 </example> 
    public int Id { get; set; }

    /// <summary>
    /// Gets and sets Unique name of product entity
    /// </summary>
    /// <example> "Soap" </example>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets and sets the price of product ordered .
    /// </summary>
    /// <example> 10.50 </example>
    [Required]
    public decimal Price { get; set; }
}
