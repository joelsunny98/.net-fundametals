using System.Globalization;

namespace RetailStore;

public static class DecimalValue
{
    public static string AddDecimalPoints(this decimal TotalAmount)
    {
        decimal amount = Math.Round(TotalAmount,2);
        string total = amount.ToString("C",CultureInfo.CurrentCulture);
        return total;
    }
}
