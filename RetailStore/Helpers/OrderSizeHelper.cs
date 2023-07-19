using RetailStore.Eums;

namespace RetailStore.Helpers
{
    public static class OrderSizeHelper
    {
        public static string CalculateOrderSize(int count)
        {
            if (count >= 10)
            {
                return Enum.GetName(typeof(Enums.OrderSize), 0);
            }
            else if (count >= 5)
            {
                return Enum.GetName(typeof(Enums.OrderSize), 1);
            }
            else if (count >= 2)
            {
                return Enum.GetName(typeof(Enums.OrderSize), 2);
            }
            else
            {
                return Enum.GetName(typeof(Enums.OrderSize), 3);
            }

        }
    }
}
