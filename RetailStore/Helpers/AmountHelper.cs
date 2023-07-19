using RetailStore.Dtos;

namespace RetailStore.Helpers;

/// <summary>
/// Helper to calculate total value
/// </summary>
public static class AmountHelper
{
    /// <summary>
    /// method to calculate total price
    /// </summary>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static AmountDto CalculateTotalValue(decimal price, int quantity)
    {
        var totalAmount = price * quantity;

        decimal discountValue = 0;

        if (totalAmount >= 1000 && totalAmount < 2000)
        {
            discountValue = totalAmount / 10;
        }
        else if (totalAmount >= 2000 && totalAmount < 3000)
        {
            discountValue = totalAmount / 20;
        }
        else if (totalAmount >= 3000 && totalAmount < 4000)
        {
            discountValue = totalAmount / 30;
        }
        else if (totalAmount >= 4000)
        {
            discountValue = totalAmount / 40;
        }

        var discountedAmount = totalAmount - discountValue;

        return new AmountDto
        {
            DiscountedAmount = discountedAmount,
            DiscountValue = discountValue
        };
    }
}
