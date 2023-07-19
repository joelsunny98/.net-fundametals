using System.ComponentModel.DataAnnotations;

namespace RetailStore.Model;

/// <summary>
/// Entity for customers
/// </summary>
public class Customer: DomainAudit
{

    /// <summary>
    /// Gets and sets Unique Identification number of the customer entity
    /// </summary>
    /// <example> 1 </example> 
    public int Id { get; set; }

    /// <summary>
    /// Gets and sets Name of the customer to be referred
    /// </summary>
    /// <example> "Aleena" </example> 
    public string Name { get; set; }

    /// <summary>
    /// Gets and sets Unique Identification number as  phone number of of the customer entity
    /// </summary>
    /// <example> 9878767567 </example> 
    [Required]
    public long? PhoneNumber { get; set; }
}
