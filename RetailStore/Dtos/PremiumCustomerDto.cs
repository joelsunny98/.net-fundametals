using MediatR;

namespace RetailStore.Requests.CustomerManagement;

public class PremiumCustomerDto
{

    /// <summary>
    /// Gets and sets Unique Identification number of the customer entity
    /// </summary>
    /// <example> 1 </example> 
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets and sets Unique PremiumCode of the customer entity
    /// </summary>
    /// <example> hjghj576 </example> 
    public string PremiumCode { get; set; }

    /// <summary>
    /// Gets and sets the Customer Name
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets and sets the Phone Number of the Customer
    /// </summary>
    public long PhoneNumber { get; set; }

    /// <summary>
    /// Gets and sets the total purchase amount of the Customer
    /// </summary>
    public decimal TotalPurchaseAmount { get; set; }
}
