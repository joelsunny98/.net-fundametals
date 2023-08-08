using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;
using Microsoft.Extensions.Logging;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Query to get Customers by order
/// </summary>
public class GetOrderByCustomerQuery : IRequest<List<CustomerByOrderDto>>
{
}

/// <summary>
/// Handles Get Order by customer query
/// </summary>
public class GetOrderByCustomerQueryHandler : IRequestHandler<GetOrderByCustomerQuery, List<CustomerByOrderDto>>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailDbContext and Logger
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetOrderByCustomerQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetOrderByCustomerQuery> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

    }

    /// <summary>
    /// Fetches list of customer by order
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Customer by Order</returns>
    public async Task<List<CustomerByOrderDto>> Handle(GetOrderByCustomerQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Orders.Include(t => t.Customer).GroupBy(c => c.CustomerId).Select(g => new CustomerByOrderDto
        {
            CustomerName = g.FirstOrDefault().Customer.Name,
            TotalOrders = g.Count()
        }).ToListAsync(cancellationToken);

        _logger.LogInformation(LogMessage.GetAllItems, result.Count);
        return result;
    }
}
