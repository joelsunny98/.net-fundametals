using RetailStore.Constants;
using RetailStore.Eums;

namespace RetailStore.Helpers
{
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
            if (count >= MagicNumber.Ten)
            {
                return Enums.OrderSize.Large.ToString();
            }
            else if (count >= MagicNumber.Five)
            {
                return Enums.OrderSize.Medium.ToString();
            }
            else if (count >= MagicNumber.Two)
            {
                return Enums.OrderSize.Small.ToString();
            }
            else
            {
                return Enums.OrderSize.SingleItem.ToString();
            }

        }
    }
}
