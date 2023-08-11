using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Helpers;
using Microsoft.Extensions.Logging;

namespace RetailStore.Requests.CustomerManagement;

/// <summary>
/// Query to get premium customers
/// </summary>
public class GetPremiumCustomersQuery : IRequest<List<PremiumCustomerDto>>
{
}

/// <summary>
/// Handler for get premium customer query 
/// </summary>
public class GetPremiumCustomersQueryHandler : IRequestHandler<GetPremiumCustomersQuery, List<PremiumCustomerDto>>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly IPremiumCodeService _premiumCodeService;

    /// <summary>
    /// Injects Dependencies
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetPremiumCustomersQueryHandler(IRetailStoreDbContext dbContext, IPremiumCodeService premiumCodeService)
    {
        _dbContext = dbContext;
        _premiumCodeService = premiumCodeService;
    }

    /// <summary>
    /// Fetches Premium Customers
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<PremiumCustomerDto>> Handle(GetPremiumCustomersQuery query, CancellationToken cancellationToken)
    {
        var allCustomers = await _dbContext.Orders
            .GroupBy(order => order.CustomerId)
            .Select(group => new PremiumCustomerDto
            {
                CustomerId = group.Key,
                CustomerName = group.First().Customer.Name,
                PhoneNumber = (long)group.First().Customer.PhoneNumber,
                TotalPurchaseAmount = group.Sum(order => order.TotalAmount)
            })
            .ToListAsync(cancellationToken);

        var premiumCustomers = PremiumCustomerHelper.GetPremiumCustomers(allCustomers);

        foreach (var premiumCustomer in premiumCustomers)
        {
            premiumCustomer.PremiumCode = _premiumCodeService.GeneratePremiumCode();
        }

        return premiumCustomers;
    }
}
