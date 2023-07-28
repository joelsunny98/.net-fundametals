namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object for storing customer information.
/// </summary>
public class CustomerDto
{
    /// <summary>
    /// Gets or sets the customer's name.
    /// </summary>
    /// <example> "Rahul" </example>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    /// <example> 9878789778 </example>
    public long PhoneNumber { get; set; }
}
