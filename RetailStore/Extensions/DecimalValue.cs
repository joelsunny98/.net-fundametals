using System.Globalization;

namespace RetailStore;

public static class DecimalValue
{
    public static string ConvertToCurrencyString(this decimal TotalAmount)
    {
        string total = TotalAmount.ToString("C",CultureInfo.CurrentCulture);
        return total;
    }
}
