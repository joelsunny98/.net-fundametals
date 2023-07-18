using RetailStore.Eums;

namespace RetailStore.Helpers
{
    public static class OrderSizeHelper
    {
        public static string CalculateOrderSize(int count)
        {
            int orderSize = 0;

            if (count >= 10)
                orderSize = 0;
            else if (count >= 5)
                orderSize = 1;
            else if (count == 1)
            orderSize = 2;

            return Enum.GetName(typeof(Enums.OrderSize), orderSize);
        }
    }
}
