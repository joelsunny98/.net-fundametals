namespace RetailStore.Dtos;

/// <summary>
/// Data Transfer Object for storing customer information.
/// </summary>
public class CustomerDto
{
    /// <summary>
    /// Gets or sets the customer's name.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public long PhoneNumber { get; set; }
}
