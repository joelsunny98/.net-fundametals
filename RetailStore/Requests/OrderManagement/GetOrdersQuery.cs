using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Query to fetch all Orders
/// </summary>
public class GetOrdersQuery : IRequest<List<OrderDto>>
{
}

/// <summary>
/// Handler for the Get Orders Query
/// </summary>
public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects the RetailStoreDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger">The logger instance.</param>
    public GetOrdersQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetOrdersQuery> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

    }

    /// <summary>
    /// Fetches all orders
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of Orders</returns>
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Orders.Include(e => e.Details).ThenInclude(e => e.Product).Include(e => e.Customer)
            .Select(e => new OrderDto()
            {
                CustomerName = e.Customer.Name,
                Amount = (e.TotalAmount + e.Discount).ConvertToCurrencyString(),
                Discount = e.Discount.ConvertToCurrencyString(),
                TotalAmount = e.TotalAmount.ConvertToCurrencyString(),
                Details = e.Details.Select(d => new OrderDetailDto()
                {
                    ProductName = d.Product.Name,
                    Quantity = d.Quantity
                }).ToList()
            }).ToListAsync();

        _logger.LogInformation(LogMessage.GetAllItems, result.Count);
        return result;
    }
}
