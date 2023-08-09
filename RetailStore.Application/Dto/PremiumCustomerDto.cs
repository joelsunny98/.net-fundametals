using MediatR;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Data Transfer Object (DTO) class for representing a premium customer.
/// </summary>
public class PremiumCustomerDto
{
    /// <summary>
    /// Gets or sets the unique identification number of the customer entity.
    /// </summary>
    /// <example>1</example>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the unique PremiumCode of the customer entity.
    /// </summary>
    /// <example>hjghj576</example>
    public string PremiumCode { get; set; }

    /// <summary>
    /// Gets or sets the customer's name.
    /// </summary>
    /// <example>"Rajesh"</example>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    /// <example>9898989898</example>
    public long PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the total purchase amount of the customer.
    /// </summary>
    /// <example>1000.50</example>
    public decimal TotalPurchaseAmount { get; set; }
}
