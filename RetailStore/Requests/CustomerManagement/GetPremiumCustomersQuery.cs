using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RetailStore.Dtos;
using RetailStore.Persistence;
using RetailStore.Services;
using RetailStore.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RetailStore.Requests.CustomerManagement
{
    public class GetPremiumCustomersQuery : IRequest<List<PremiumCustomerDto>>
    {
    }

    public class GetPremiumCustomersQueryHandler : IRequestHandler<GetPremiumCustomersQuery, List<PremiumCustomerDto>>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger<GetPremiumCustomersQueryHandler> _logger;

        public GetPremiumCustomersQueryHandler(RetailStoreDbContext dbContext, ILogger<GetPremiumCustomersQueryHandler> logger) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<PremiumCustomerDto>> Handle(GetPremiumCustomersQuery query, CancellationToken cancellationToken)
        {
            var premiumCodeService = new PremiumCodeService();

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

            var premiumCustomers = PremiumCustomerHelper.GetPremiumCustomers(allCustomers);
            foreach (var premiumCustomer in premiumCustomers)
            {
                premiumCustomer.PremiumCode = premiumCodeService.GeneratePremiumCode();
            }

            _logger.LogInformation("Processed {customerCount} customers for premium code generation.", premiumCustomers.Count);

            return premiumCustomers;
        }
    }
}
