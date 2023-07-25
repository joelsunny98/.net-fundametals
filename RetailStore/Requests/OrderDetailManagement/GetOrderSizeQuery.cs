using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Contracts;
using RetailStore.Helpers;

namespace RetailStore.Requests.OrderDetailManagement;

/// <summary>
/// Query to fetch the size of Order
/// </summary>
public class GetOrderSizeQuery : IRequest<string>
{
    /// <summary>
    /// Gets and sets OrderId
    /// </summary>
    public int OrderId { get; set; }
}

/// <summary>
/// Handler for Get Order size Query
/// </summary>
public class GetOrderSizeQueryHandler : IRequestHandler<GetOrderSizeQuery, string>
{
    private readonly IRetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailDbContext class and Logger
    /// </summary>
    /// <param name="dbContext"></param>
    public GetOrderSizeQueryHandler(IRetailStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Fetches Order Size
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Order Size</returns>
    public async Task<string> Handle(GetOrderSizeQuery request, CancellationToken cancellationToken)
    {
        var count = await _dbContext.OrderDetails.Where(e => e.OrderId == request.OrderId).CountAsync(cancellationToken);

        var result = OrderSizeHelper.CalculateOrderSize(count);

        return result;
    }
}
