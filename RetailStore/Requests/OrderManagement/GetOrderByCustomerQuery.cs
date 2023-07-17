using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Requests.OrderManagement
{
    public class GetOrderByCustomerQuery : IRequest<List<CustomerByOrderDto>>
    {
    }

    public class GetOrderByCustomerQueryHandler : IRequestHandler<GetOrderByCustomerQuery, List<CustomerByOrderDto>>
    {
        private readonly RetailStoreDbContext _dbContext;
        private readonly ILogger _logger;

        public GetOrderByCustomerQueryHandler(RetailStoreDbContext dbContext, ILogger<GetOrderByCustomerQuery> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

        }

        public async Task<List<CustomerByOrderDto>> Handle(GetOrderByCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Orders.Include(t => t.Customer).GroupBy(c => c.CustomerId).Select(g => new CustomerByOrderDto
            {
                CustomerName = g.FirstOrDefault().Customer.Name,
                TotalOrders = g.Count()
            }).ToListAsync();

            _logger.LogInformation("Retreived {CustomerCount} Customer", result.Count);
            return result;
        }
    }
}
