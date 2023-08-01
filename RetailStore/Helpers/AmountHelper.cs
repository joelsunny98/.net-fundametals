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

        decimal discountValue = 0;

        if (totalAmount >= Constant.Thousand && totalAmount < Constant.TwoThousand)
        {
            discountValue = totalAmount / Constant.Ten;
        }
        else if (totalAmount >= Constant.TwoThousand && totalAmount < Constant.ThreeThousand)
        {
            discountValue = totalAmount / Constant.Twenty;
        }
        else if (totalAmount >= Constant.ThreeThousand && totalAmount < Constant.FourThousand)
        {
            discountValue = totalAmount / Constant.Thirty;
        }
        else if (totalAmount >= Constant.FourThousand)
        {
            discountValue = totalAmount / Constant.Forty;
        }

        var discountedAmount = totalAmount - discountValue;

        return new AmountDto
        {
            DiscountedAmount = discountedAmount,
            DiscountValue = discountValue
        };
    }
}
