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
            if (count >= Constants.Constant.Ten)
            {
                return Enums.OrderSize.Large.ToString();
            }
            else if (count >= Constants.Constant.Five)
            {
                return Enums.OrderSize.Medium.ToString();
            }
            else if (count >= Constants.Constant.Two)
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
