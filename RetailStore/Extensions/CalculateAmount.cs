using RetailStore.Dtos;
using RetailStore.Model;
namespace RetailStore.Extensions
{
    public static class CalculateAmount
    {
        public static AmountDto TotalValue(this decimal Amount, decimal price, int quantity)
        {
            var TotalAmount = price * quantity;
            Amount += TotalAmount;

            decimal discountValue = 0;

            if (Amount >= 1000 && Amount < 2000)
                discountValue = Amount / 10;
            else if (Amount >= 2000 && Amount < 3000)
                discountValue = Amount / 20;
            else if (Amount >= 3000 && Amount < 4000)
                discountValue = Amount / 30;
            else if (Amount >= 4000)
                discountValue = Amount / 40;

            Amount -= discountValue;

            return new AmountDto
            {
                DiscountedAmount = Amount,
                DiscountValue = discountValue
            };
        }

    }
}
