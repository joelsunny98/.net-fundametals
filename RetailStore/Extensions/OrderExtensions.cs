using RetailStore.Persistence;

namespace RetailStore.Extentions;

public static class OrderExtensions
{
    public static decimal TotalRevenue(this decimal price, int quantity) 
    {
        return price * quantity;
    }
}
