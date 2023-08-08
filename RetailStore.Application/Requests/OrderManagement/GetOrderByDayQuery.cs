using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Constants;
using RetailStore.Contracts;
using RetailStore.Dtos;
using Microsoft.Extensions.Logging;

namespace RetailStore.Requests.OrderManagement;

/// <summary>
/// Query to fetch all the order made today
/// </summary>
public class GetOrderByDayQuery : IRequest<List<OrderDto>>
{
}

/// <summary>
/// Handler for Get Order by Day Query
/// </summary>
public class GetOrderByDayQueryHandler : IRequestHandler<GetOrderByDayQuery, List<OrderDto>>
{
    private readonly IRetailStoreDbContext _dbContext;
    private readonly ILogger _logger;

    /// <summary>
    /// Injects RetailDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public GetOrderByDayQueryHandler(IRetailStoreDbContext dbContext, ILogger<GetOrderByDayQuery> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

    }

    /// <summary>
    /// Fetches all the orders made today
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<OrderDto>> Handle(GetOrderByDayQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Orders.Include(e => e.Details).Where(e => e.CreatedOn.Date == DateTime.UtcNow.Date).Select(e => new OrderDto
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
        }).ToListAsync(cancellationToken);

        _logger.LogInformation(LogMessage.OrderForTheDay, result.Count);
        return result;
    }
}
