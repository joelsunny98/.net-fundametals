using RetailStore.Persistence;

namespace RetailStore.Extentions;

public static class OrderExtensions
{
    public static double TotalRevenue(this double price, int quantity) 
    {
        return price * quantity;
    }
}
