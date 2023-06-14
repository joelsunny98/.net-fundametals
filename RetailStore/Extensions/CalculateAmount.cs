using RetailStore.Model;

namespace RetailStore.Extensions
{
    public  static class CalculateAmount
    {
        public static double TotalValue(this double Amount ,double price, int quantity)
        {
            var TotalAmount = price * quantity;
            Amount += TotalAmount;
            return Amount;
        }
    }
}
