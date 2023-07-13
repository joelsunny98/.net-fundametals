using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Features.OrderManagement.Queries
{
    public class GetOrderByCustomerQuery: IRequest<List<CustomerByOrderDto>>
    {
    }

    public class GetOrderByCustomerQueryHandler: IRequestHandler<GetOrderByCustomerQuery, List<CustomerByOrderDto>> 
    {
        private readonly RetailStoreDbContext _dbContext;

        public GetOrderByCustomerQueryHandler(RetailStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CustomerByOrderDto>> Handle(GetOrderByCustomerQuery request, CancellationToken cancellationToken) 
        {
            var result = await _dbContext.Orders.Include(t => t.Customer).GroupBy(c => c.CustomerId).Select(g => new CustomerByOrderDto
            {
                CustomerName = g.FirstOrDefault().Customer.Name,
                TotalOrders = g.Count()
            }).ToListAsync();

            return result;
        }
    }
}
