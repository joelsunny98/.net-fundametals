namespace RetailStore.Dtos;

public class CustomerDto
{
    /// <summary>
    /// Gets and sets the Customer Name
    /// </summary>
    /// <example> "Ravi K" </example>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets and sets the Phone Number of the Customer
    /// </summary>
    /// <example> 9878789778 </example>
    public long PhoneNumber { get; set; }
}
