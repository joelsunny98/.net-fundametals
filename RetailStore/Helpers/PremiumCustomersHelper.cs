using System.Collections.Generic;
using System.Linq;
using RetailStore.Dtos;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Helpers;

public static class PremiumCustomerHelper
{
    public static List<PremiumCustomerDto> GetPremiumCustomers(List<PremiumCustomerDto> customers)
    {
        var premiumCustomers = customers.Where(customer => customer.TotalPurchaseAmount > 5000).ToList();
        return premiumCustomers;
    }
}
