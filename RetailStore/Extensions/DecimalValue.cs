using RetailStore.Constants;
using System.Globalization;

namespace RetailStore;

/// <summary>
/// Extention to convert a decimal to currency
/// </summary>
public static class DecimalValue
{
    /// <summary>
    /// Method to convert decimal to currency
    /// </summary>
    /// <param name="TotalAmount"></param>
    /// <returns></returns>
    public static string ConvertToCurrencyString(this decimal TotalAmount)
    {
        string total = TotalAmount.ToString(Constant.CurrencyFormatSpecifier, CultureInfo.CurrentCulture);
        return total;
    }
}
