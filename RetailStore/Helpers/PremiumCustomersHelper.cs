using System.Collections.Generic;
using System.Linq;
using RetailStore.Dtos;
using RetailStore.Requests.CustomerManagement;

namespace RetailStore.Helpers;

/// <summary>
/// Helper class for filtering a list of customers and returning only the premium customers.
/// </summary>
public static class PremiumCustomerHelper
{

    /// <summary>
    /// returns  the premium customers based on a specified total purchase amount threshold.
    /// </summary>
    /// <param name="customers">The list of customers to be filtered</param>
    /// <returns>A List of PremiumCustomerDto containing the premium customers</returns>
    public static List<PremiumCustomerDto> GetPremiumCustomers(List<PremiumCustomerDto> customers)
    {
        var premiumCustomers = customers.Where(customer => customer.TotalPurchaseAmount > 5000).ToList();
        return premiumCustomers;
    }
}
