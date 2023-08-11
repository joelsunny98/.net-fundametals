using RetailStore.Constants;
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

        decimal discountValue = default(decimal);

        if (totalAmount >= Constant.TenPercentDiscountMinValue && totalAmount < Constant.TenPercentDiscountMaxValue)
        {
            discountValue = totalAmount / Constant.TenPercent;
        }
        else if (totalAmount >= Constant.TenPercentDiscountMaxValue && totalAmount < Constant.TwentyPercentDiscountMaxValue)
        {
            discountValue = totalAmount / Constant.TwentyPercent;
        }
        else if (totalAmount >= Constant.TwentyPercentDiscountMaxValue && totalAmount < Constant.ThirtyPercentDiscountMaxValue)
        {
            discountValue = totalAmount / Constant.ThirtyPercent;
        }
        else if (totalAmount >= Constant.ThirtyPercentDiscountMaxValue)
        {
            discountValue = totalAmount / Constant.FortyPercent;
        }

        var discountedAmount = totalAmount - discountValue;

        return new AmountDto
        {
            DiscountedAmount = discountedAmount,
            DiscountValue = discountValue
        };
    }
}
