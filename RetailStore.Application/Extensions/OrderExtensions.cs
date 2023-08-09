namespace RetailStore.Extentions;

/// <summary>
/// Extention to generate total revenue
/// </summary>
public static class OrderExtensions
{
    /// <summary>
    /// Method to generate total revenue
    /// </summary>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static decimal TotalRevenue(this decimal price, int quantity)
    {
        return price * quantity;
    }
}
