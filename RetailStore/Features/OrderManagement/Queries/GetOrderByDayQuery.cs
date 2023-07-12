using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Persistence;

namespace RetailStore.Features.OrderManagement.Queries;

/// <summary>
/// Query to fetch all the order made today
/// </summary>
public class GetOrderByDayQuery: IRequest<List<OrderDto>>
{
}

/// <summary>
/// Handler for Get Order by Day Query
/// </summary>
public class GetOrderByDayQueryHandler : IRequestHandler<GetOrderByDayQuery, List<OrderDto>> 
{
    private readonly RetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailDbContext class
    /// </summary>
    /// <param name="dbContext"></param>
    public GetOrderByDayQueryHandler(RetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
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
        }).ToListAsync();

        return result;
    }
}
