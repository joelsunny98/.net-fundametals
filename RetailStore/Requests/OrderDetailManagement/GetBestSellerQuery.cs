using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Extentions;
using RetailStore.Persistence;

namespace RetailStore.Requests.OrderDetailManagement;

/// <summary>
/// Query to get best seller
/// </summary>
public class GetBestSellerQuery : IRequest<BestSellerDto>
{
}

/// <summary>
/// Handler for Get Best Seller Query
/// </summary>
public class GetBestSellerQueryHandler : IRequestHandler<GetBestSellerQuery, BestSellerDto>
{
    private readonly RetailStoreDbContext _dbContext;

    /// <summary>
    /// Injects RetailDbContext cladd
    /// </summary>
    /// <param name="dbContext"></param>
    public GetBestSellerQueryHandler(RetailStoreDbContext dbContext) 
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Fethes best seller product
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BestSellerDto> Handle(GetBestSellerQuery request, CancellationToken cancellationToken) 
    {
        var bestSeller = await _dbContext.OrderDetails.Include(t => t.Product).GroupBy(c => c.ProductId).OrderByDescending(g => g.Sum(q => q.Quantity)).Select(g => new BestSellerDto
        {
            ProductId = g.Key,
            Quantity = g.Sum(od => od.Quantity),
            ProductName = g.First().Product.Name,
            Price = g.First().Product.Price,
            TotalRevenue = g.First().Product.Price.TotalRevenue(g.Sum(od => od.Quantity))
        }).FirstOrDefaultAsync(cancellationToken);

        return bestSeller;
    }
    
}