using MediatR;
using Microsoft.EntityFrameworkCore;
using RetailStore.Dtos;
using RetailStore.Extentions;
using RetailStore.Persistence;

namespace RetailStore.Requests.OrderDetailManagement;

public class GetBestSellerQuery : IRequest<BestSellerDto>
{
}

public class GetBestSellerQueryHandler : IRequestHandler<GetBestSellerQuery, BestSellerDto>
{
    private readonly RetailStoreDbContext _dbContext;

    public GetBestSellerQueryHandler(RetailStoreDbContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public async Task<BestSellerDto> Handle(GetBestSellerQuery request, CancellationToken cancellationToken) 
    {
        var bestSeller = await _dbContext.OrderDetails.Include(t => t.Product).GroupBy(c => c.ProductId).OrderByDescending(g => g.Sum(q => q.Quantity)).Select(g => new BestSellerDto
        {
            ProductId = g.Key,
            Quantity = g.Sum(od => od.Quantity),
            ProductName = g.First().Product.Name,
            Price = g.First().Product.Price,
            TotalRevenue = g.First().Product.Price.TotalRevenue(g.Sum(od => od.Quantity))
        }).FirstOrDefaultAsync();

        return bestSeller;
    }
    
}