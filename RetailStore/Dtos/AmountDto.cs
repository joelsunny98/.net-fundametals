namespace RetailStore.Dtos;

public class AmountDto
{
    /// <summary>
    /// Gets and sets Discounted Amount
    /// </summary>
    /// <example> 100.50 </example>
    public decimal DiscountedAmount { get; set; }

    /// <summary>
    /// Gets and sets Discount Value
    /// </summary>
    /// <example> 10.50 </example>
    public decimal DiscountValue { get; set; }
}
