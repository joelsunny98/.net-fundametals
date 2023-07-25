using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailStore.Dtos;
using RetailStore.Persistence;
using RetailStore.Services;
using RetailStore.Helpers; // Add the using statement for the helper class
using Microsoft.EntityFrameworkCore;

namespace RetailStore.Requests.CustomerManagement
{
    public class GetPremiumCustomersQuery : IRequest<List<PremiumCustomerDto>>
    {
    }

    public class GetPremiumCustomersQueryHandler : IRequestHandler<GetPremiumCustomersQuery, List<PremiumCustomerDto>>
    {
        private readonly RetailStoreDbContext _dbContext;

        public GetPremiumCustomersQueryHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PremiumCustomerDto>> Handle(GetPremiumCustomersQuery query, CancellationToken cancellationToken)
        {
            var premiumCodeService = new PremiumCodeService(); // Create a new instance of the PremiumCodeService

            var allCustomers = await _dbContext.Orders
                .GroupBy(order => order.CustomerId)
                .Select(group => new PremiumCustomerDto
                {
                    CustomerId = group.Key,
                    CustomerName = group.First().Customer.Name,
                    PhoneNumber = (long)group.First().Customer.PhoneNumber,
                    TotalPurchaseAmount = group.Sum(order => order.TotalAmount)
                })
                .ToListAsync();

            var premiumCustomers = PremiumCustomerHelper.GetPremiumCustomers(allCustomers); // Use the helper to filter premium customers

            // Generate unique premium codes for premium customers using the new instance of PremiumCodeService
            foreach (var premiumCustomer in premiumCustomers)
            {
                premiumCustomer.PremiumCode = premiumCodeService.GeneratePremiumCode();
            }

            return premiumCustomers;
        }
    }
}
