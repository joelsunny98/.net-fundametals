using RetailStore.Constants;
using RetailStore.Eums;

namespace RetailStore.Helpers;

/// <summary>
/// Helper for Order Size
/// </summary>
public static class OrderSizeHelper
{
    /// <summary>
    /// Method to get Order Size
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string CalculateOrderSize(int count)
    {
        if (count >= Constant.LargeOrderSize)
        {
            return Enums.OrderSize.Large.ToString();
        }
        else if (count >= Constant.MediumOrderSize)
        {
            return Enums.OrderSize.Medium.ToString();
        }
        else if (count >= Constant.SmallOrderSize)
        {
            return Enums.OrderSize.Small.ToString();
        }
        else
        {
            return Enums.OrderSize.SingleItem.ToString();
        }
    }
}
